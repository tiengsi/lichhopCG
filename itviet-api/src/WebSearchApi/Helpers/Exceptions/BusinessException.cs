using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dtos;

namespace WebApi.Helpers.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }

        public BusinessException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
