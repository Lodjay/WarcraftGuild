using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.Exceptions
{
    public class BadResponseException : Exception
    {
        public IApiResponse ResponseMessage;
        public HttpStatusCode StatusCode;

        public BadResponseException(string message, HttpStatusCode statusCode, IApiResponse responseMessage) : base(message)
        {
            ResponseMessage = responseMessage;
            StatusCode = statusCode;
        }

        public BadResponseException(string message, IApiResponse responseMessage) : base(message)
        {
            ResponseMessage = responseMessage;
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public BadResponseException(IApiResponse responseMessage) : base()
        {
            ResponseMessage = responseMessage;
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }

}
