using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Services.Services;
using PhotoGalleryAPI.Shared.DTOs;
using PhotoGalleryAPI.Storage;
using System.Security.Claims;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;

        public AlbumsController(IAlbumService albumService, IPhotoService photoService)
        {
            _albumService = albumService;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IResponse<IEnumerable<AlbumDTO>>>> GetAll([FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            var response = await _albumService.GetAllAsync(null, itemsPerPage, selectedPage);
            return Ok(response);
        }

        [HttpGet("My")]
        [Authorize]
        public async Task<ActionResult<IResponse<IEnumerable<AlbumDTO>>>> GetAllByPersonId([FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            var response = string.IsNullOrWhiteSpace(User.FindFirstValue(ClaimTypes.NameIdentifier)) ?
                throw new ArgumentException("Invalid person ID argument") :
                await _albumService.GetAllAsync(x => x.CreatedByPersonId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), itemsPerPage, selectedPage);

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IResponse<AlbumDTO>>> Create([FromBody] AlbumDTO albumDTO)
        {
            var response = await _albumService.AddAsync(albumDTO);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<IResponse<AlbumDTO>>> Delete([FromQuery] string albumId = "")
        {
            var photoResponse = await _photoService.GetAllAsync(x => x.AlbumId == Guid.Parse(albumId));

            if(!photoResponse.Success)
                return Ok(photoResponse);

            foreach (var photo in photoResponse.Data)
                await PhotoStorage.DeleteFileAsync(photo.Filename);

            var response = await _albumService.DeleteAsync(albumId);
            return Ok(response);
        }

        [HttpGet("Album/{albumId}")]
        public async Task<ActionResult<IResponse<AlbumDTO>>> GetById(string albumId = "")
        {
            var response = await _albumService.GetByIdAsync(albumId);
            return Ok(response);
        }
    }
}
