using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Libawai.API.Helpers
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchingMediaTypeAttribute:Attribute,IActionConstraint
    {
        private readonly string _requestHeaderToMatch;
        private readonly string[] _mediaTypes;

        public RequestHeaderMatchingMediaTypeAttribute(
            string requestHeaderToMatch, 
            string[] mediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch;
            _mediaTypes = mediaTypes;
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }

            return _mediaTypes.Select(mediaType 
                => string.Equals(requestHeaders[_requestHeaderToMatch].ToString(),
                    mediaType, StringComparison.OrdinalIgnoreCase))
                .Any(mediaTypeMatches => mediaTypeMatches);
        }

        public int Order { get; } = 0;
    }
}
