using AutoMapper;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.DAL.Repositories;
using PhotoGalleryAPI.Services.Interfaces;
using PhotoGalleryAPI.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Services.Services
{
    public interface ILikeService : IService<Like, LikeDTO, string>
    {
    }

    public class LikeService : BaseService<Like, LikeDTO, string>, ILikeService
    {
        private readonly IRepository<Like> _likeRepo;
        private readonly IMapper _mapper;

        public LikeService(IRepository<Like> likeRepo, IMapper mapper) : base(likeRepo, mapper)
        {
            _likeRepo = likeRepo;
            _mapper = mapper;
        }
    }
}
