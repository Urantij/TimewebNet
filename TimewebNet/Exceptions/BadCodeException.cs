using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TimewebNet.Exceptions
{
    public class BadCodeException : Exception
    {
        public HttpStatusCode Code { get; private set; }
        public string Content { get; private set; }
        public string Where { get; private set; }

        public BadCodeException(HttpStatusCode code, string content, string where)
            : base($"Плохой код вернулся. {code} ({where})\n{content}")
        {
            Code = code;
            Content = content;
            Where = where;
        }
    }
}