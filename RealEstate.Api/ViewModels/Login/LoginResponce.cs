using System.Text.Json.Serialization;

namespace RealEstate.Api.ViewModels.User
{
    public class LoginResponce
    {
        [JsonPropertyName("accessToken")]
        public required string Token { get; set; }
    }
}
