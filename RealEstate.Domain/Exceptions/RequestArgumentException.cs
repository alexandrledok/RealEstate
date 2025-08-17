using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Exceptions
{
    public class RequestArgumentException : ArgumentException
    {
        public RequestArgumentException(ModelStateDictionary modelState):
            this(string.Join("\r\n", modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)))
        {
            
        }

        public RequestArgumentException(string? message) : base(message)
        {
        }
    }
}
