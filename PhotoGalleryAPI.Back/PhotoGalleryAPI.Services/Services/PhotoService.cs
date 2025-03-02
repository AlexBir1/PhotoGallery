using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.DAL.Repositories;
using PhotoGalleryAPI.Services.Interfaces;
using PhotoGalleryAPI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Services.Services
{
    public interface IPhotoService : IService<Photo, PhotoDTO, string>
    {
    }

    public class PhotoService : BaseService<Photo, PhotoDTO, string>, IPhotoService
    {
        private readonly IRepository<Photo> _photoRepo;
        private readonly IMapper _mapper;

        public PhotoService(IRepository<Photo> photoRepo, IMapper mapper) : base(photoRepo, mapper)
        {
            _photoRepo = photoRepo;
            _mapper = mapper;
        }

        public override async Task<IResponse<IEnumerable<PhotoDTO>>> GetAllAsync(Expression<Func<Photo, bool>> exp = null, int itemsPerPage = 1, int selectedPage = 1)
        {
            var response = exp != null ? 
                await _photoRepo.GetAllAsync(itemsPerPage, selectedPage, exp, x => x.Include(y => y.Likes)) as DbResponse<IEnumerable<Photo>> : 
                await _photoRepo.GetAllAsync(itemsPerPage, selectedPage,null,x=>x.Include(y=>y.Likes)) as DbResponse<IEnumerable<Photo>>;
            if (!response.Success)
            {
                return APIResponse<IEnumerable<PhotoDTO>>.FailureResponse(response.Errors);
            }

            var dtos = _mapper.Map<IEnumerable<PhotoDTO>>(response.Data);
            return APIResponse<IEnumerable<PhotoDTO>>.SuccessPagedResponse(dtos, itemsPerPage, selectedPage, response.ItemsCount);
        }
    }
}
