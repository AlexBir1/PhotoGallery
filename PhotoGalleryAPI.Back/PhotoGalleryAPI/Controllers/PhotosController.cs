using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Services.Services;
using PhotoGalleryAPI.Shared.DTOs;
using PhotoGalleryAPI.Storage;
using System.Security.Claims;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("Album")]
        public async Task<ActionResult<IResponse<IEnumerable<PhotoDTO>>>> GetAllByAlbumId([FromQuery] string albumId = "", [FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            var response = string.IsNullOrWhiteSpace(albumId) ?
                throw new ArgumentException("Invalid album ID argument") :
                await _photoService.GetAllAsync(x => x.AlbumId == Guid.Parse(albumId), itemsPerPage, selectedPage);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("{albumId}")]
        [Authorize]
        public async Task<ActionResult<IResponse<PhotoDTO>>> AddPhoto(string albumId)
        {
            var file = Request.Form.Files[0];
            string newFilename = await PhotoStorage.UploadFileAsync(file);

            var newDto = new PhotoDTO
            {
                UploadedDate = DateTime.Now,
                Filename = newFilename,
                AlbumId = albumId
            };

            var response = await _photoService.AddAsync(newDto);

            return response.Success ? Ok(response) : BadRequest(response);

        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<IResponse<PhotoDTO>>> DeletePhoto([FromQuery] string photoId = "")
        {
            var response = await _photoService.DeleteAsync(photoId);

            if (!response.Success)
                return BadRequest(response);
            else
            {
                await PhotoStorage.DeleteFileAsync(response.Data.Filename);
                return Ok(response);
            }
        }
    }
}
