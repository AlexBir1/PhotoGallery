using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.Filters;
using PhotoGalleryAPI.Services.Services;
using PhotoGalleryAPI.Shared.Models;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPersonService _personService;

        public AuthController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("SignUp")]
        [ValidationFilter]
        public async Task<ActionResult<APIResponse<AuthorizationModel>>> SignUp([FromBody] SignUpModel model)
        {
            var result = await _personService.SignUpAsync(model);
            return result.Success ? Ok(APIResponse<AuthorizationModel>.SuccessResponse(result.Data)) : BadRequest(result.Errors);
        }

        [HttpPost("SignIn")]
        [ValidationFilter]
        public async Task<ActionResult<APIResponse<AuthorizationModel>>> SignIn([FromBody] SignInModel model)
        {
            var result = await _personService.SignInAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
