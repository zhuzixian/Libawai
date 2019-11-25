using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Libawai.API.Helpers
{
    public class UnProcessableEntityObjectResult : Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult
    {
        public UnProcessableEntityObjectResult(ModelStateDictionary modelState)
            : base(new ResourceValidationResult(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = 442;
        }

    }
}
