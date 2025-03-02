using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.Services.Services;
using PhotoGalleryAPI.Shared.DTOs;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<IResponse<LikeDTO>>> CUDLike([FromBody] LikeDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Id))
            {
                var response = await _likeService.AddAsync(dto);

                return Ok(response);
            }
            else
            {
                var likeResult = await _likeService.GetByIdAsync(dto.Id);

                if(!likeResult.Success)
                    return Ok(likeResult);

                if (likeResult.Data.IsLike == dto.IsLike)
                {
                    var response = await _likeService.DeleteAsync(dto.Id);

                    return Ok(response);
                }
                else
                {
                    var response = await _likeService.UpdateAsync(dto.Id,dto);

                    return Ok(response);
                }
            }
        }
    }
}
