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
    public interface IAlbumService : IService<Album, AlbumDTO, string>
    {
    }

    public class AlbumService : BaseService<Album, AlbumDTO, string>, IAlbumService
    {
        private readonly IRepository<Album> _albumRepo;
        private readonly IMapper _mapper;

        public AlbumService(IRepository<Album> albumRepo, IMapper mapper) : base(albumRepo, mapper)
        {
            _albumRepo = albumRepo;
            _mapper = mapper;
        }

        public override async Task<IResponse<IEnumerable<AlbumDTO>>> GetAllAsync(Expression<Func<Album, bool>> exp = null, int itemsPerPage = 1, int selectedPage = 1)
        {
            var result = await _albumRepo.GetAllAsync(itemsPerPage, selectedPage, exp, x => x.Include(y => y.Photos.Take(1))) as DbResponse<IEnumerable<Album>>;

            if (!result.Success)
                return APIResponse<IEnumerable<AlbumDTO>>.FailureResponse(result.Errors);

            return APIResponse<IEnumerable<AlbumDTO>>.SuccessPagedResponse(_mapper.Map<IEnumerable<AlbumDTO>>(result.Data),itemsPerPage,selectedPage,result.ItemsCount);
        }
    }
}
