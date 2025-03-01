using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Services.Services;
using PhotoGalleryAPI.Shared.DTOs;
using System.Security.Claims;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumsController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<ActionResult<IResponse<IEnumerable<AlbumDTO>>>> GetAll([FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            var response = await _albumService.GetAllAsync(null, itemsPerPage, selectedPage);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("My")]
        [Authorize]
        public async Task<ActionResult<IResponse<IEnumerable<AlbumDTO>>>> GetAllByPersonId([FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            var response = string.IsNullOrWhiteSpace(User.FindFirstValue(ClaimTypes.NameIdentifier)) ?
                throw new ArgumentException("Invalid person ID argument") :
                await _albumService.GetAllAsync(x => x.CreatedByPersonId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), itemsPerPage, selectedPage);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IResponse<AlbumDTO>>> Create([FromBody] AlbumDTO albumDTO)
        {
            var response = await _albumService.AddAsync(albumDTO);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<IResponse<AlbumDTO>>> Delete([FromQuery] string albumId = "")
        {
            var response = await _albumService.DeleteAsync(albumId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
