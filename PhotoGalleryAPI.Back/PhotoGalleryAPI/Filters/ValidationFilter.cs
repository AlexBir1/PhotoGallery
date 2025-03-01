
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoGalleryAPI.BaseResponse.Responses;

namespace PhotoGalleryAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(APIResponse<string>.FailureResponse(context.ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            base.OnActionExecuting(context);
        }
    }
}
