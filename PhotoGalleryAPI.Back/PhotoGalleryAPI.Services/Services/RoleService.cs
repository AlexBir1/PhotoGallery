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
    public interface IRoleService : IService<Role, RoleDTO, string>
    {
    }

    public class RoleService : BaseService<Role, RoleDTO, string>, IRoleService
    {
        private readonly IRepository<Role> _roleRepo;
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> roleRepo, IMapper mapper) : base(roleRepo, mapper)
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
        }
    }
}
