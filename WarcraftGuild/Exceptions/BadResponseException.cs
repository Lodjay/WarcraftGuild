using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarcraftGuild.BlizzardApi.Interfaces;

namespace WarcraftGuild.Exceptions
{
    public class BadResponseException : Exception
    {
        public IApiResponse ResponseMessage;

        public BadResponseException(string message, IApiResponse responseMessage) : base(message)
        {
            ResponseMessage = responseMessage;
        }

        public BadResponseException(IApiResponse responseMessage) : base()
        {
            ResponseMessage = responseMessage;
        }
    }

}
