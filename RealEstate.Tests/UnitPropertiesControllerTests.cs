using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Api.Controllers;
using RealEstate.Api.ViewModels;
using RealEstate.Api.ViewModels.Properties;
using RealEstate.Api.ViewModels.Spaces;
using RealEstate.Domain;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.DTO;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Tests
{
    public class UnitPropertiesControllerTests
    {
        private readonly Mock<IPropertyService> _serviceMock = new();
        private readonly IMapper _mapper;
        private readonly PropertiesController _controller;

        public UnitPropertiesControllerTests()
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            var config = new MapperConfiguration((IMapperConfigurationExpression cfg) =>
            {
                cfg.AddProfile(new DomainMapper());
                cfg.AddProfile(new ViewModelsMapper());
            }, loggerFactory);

            _mapper = config.CreateMapper();


            _serviceMock = new Mock<IPropertyService>();
            _controller = new PropertiesController(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenExists()
        {
            // Arrange
            var dto = new PropertyDto { Id = 1 };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<PropertyVM>(result.Value);
        }

        [Fact]
        public async Task GetById_Throws_WhenInvalidId()
        {
            // Act & Assert
            await Assert.ThrowsAsync<RequestArgumentException>(() => _controller.GetById(0));
        }

        [Fact]
        public async Task GetById_ThrowsNotFound_WhenNotExists()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((PropertyDto)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(999));
        }

        [Fact]
        public async Task Create_Throws_WhenModelInvalid()
        {
            var vm = new PropertyVM();
            _controller.ModelState.AddModelError("Price", "Required");

            await Assert.ThrowsAsync<RequestArgumentException>(() => _controller.Create(vm));
        }

        [Fact]
        public async Task GetById_ThrowsNotFound_WhenNullReturned()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((PropertyDto?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(77));
        }


        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetByFilters(null, null, null, 1, 10, "asc"))
                        .ReturnsAsync(new List<PropertyDto> { new PropertyDto { Id = 9 } });

            var result = await _controller.GetAll(null, null, null) as OkObjectResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PropertyVM>>(result.Value);
            Assert.Single(model);
        }


        [Fact]
        public async Task Create_ReturnsOk_WhenValid()
        {
            var vm = new PropertyVM
            {
                Address = "123 Main St",
                Type = "house",
                Description = "A beautiful house",
                Spaces = new List<SpaceVM>() {
                    new SpaceVM{
                        Type = "bedroom",
                        Size = 20.5f,
                        Description = "Master bedroom"
                    },
                    new SpaceVM{
                        Type = "kitchen",
                        Description = "Modern kitchen",
                        Size = 15.0f
                    },
                    new SpaceVM{
                        Type = "bathroom",
                        Description = "Spacious bathroom",
                        Size = 10.0f
                    }
                },
                Price = 500
            };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<PropertyDto>())).ReturnsAsync(10);

            var result = await _controller.Create(vm) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var returned = result.Value as PropertyVM;
            Assert.Equal(10, returned!.Id);
        }

        [Fact]
        public async Task Create_Throws_WhenModelStateInvalid()
        {
            var vm = new PropertyVM();
            _controller.ModelState.AddModelError("Price", "Invalid");

            await Assert.ThrowsAsync<RequestArgumentException>(() => _controller.Create(vm));
        }


        [Fact]
        public async Task GetAll_PassesFiltersToService()
        {
            _serviceMock.Setup(s => s.GetByFilters("house", 100, 200, 1, 10, "asc"))
                        .ReturnsAsync(new List<PropertyDto>());

            var result = await _controller.GetAll("house", 100, 200);

            _serviceMock.Verify(s => s.GetByFilters("house", 100, 200, 1, 10, "asc"), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Create_Throws_WhenPriceNegative()
        {
            var vm = new PropertyVM { Price = -10 };
            _controller.ModelState.AddModelError("Price", "Price must be positive");

            await Assert.ThrowsAsync<RequestArgumentException>(() => _controller.Create(vm));
        }
    }
}