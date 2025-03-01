using AutoMapper;
using Microsoft.Extensions.Configuration;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.DAL.Repositories;
using PhotoGalleryAPI.Services.Interfaces;
using PhotoGalleryAPI.Services.JWT;
using PhotoGalleryAPI.Shared;
using PhotoGalleryAPI.Shared.DTOs;
using PhotoGalleryAPI.Shared.Models;
using PhotoGalleryAPI.Shared.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Services.Services
{
    public interface IPersonService : IService<Person, PersonDTO, string>
    {
        Task<IResponse<AuthorizationModel>> SignUpAsync(SignUpModel model);
        Task<IResponse<AuthorizationModel>> SignInAsync(SignInModel model);
    }

    public class PersonService : BaseService<Person, PersonDTO, string>, IPersonService
    {
        private readonly IRepository<Person> _personRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public PersonService(IRepository<Person> personRepo, IMapper mapper, IConfiguration config, IRepository<Role> roleRepo) : base(personRepo, mapper)
        {
            _personRepo = personRepo;
            _mapper = mapper;
            _config = config;
            _roleRepo = roleRepo;
        }

        public async Task<IResponse<AuthorizationModel>> SignUpAsync(SignUpModel model)
        {
            var checkPersonResult = await _personRepo.GetAllAsync(x => x.Username == model.Username || x.Email == model.Email, null);

            if (checkPersonResult.Success && checkPersonResult.Data.Any())
                return APIResponse<AuthorizationModel>.FailureResponse("Person with that email or username already signed up.");
            else if(!checkPersonResult.Success)
                return APIResponse<AuthorizationModel>.FailureResponse(checkPersonResult.Errors);

            var defaultRole = (await _roleRepo.GetAllAsync(null)).Data.First(r => r.RoleName == "User");

            var result = await _personRepo.AddAsync(new Person
           {
               Id = Guid.NewGuid(),
               Username = model.Username,
               Email = model.Email,
               PasswordHash = PassHandler.CreatePasswordHash(model.Password),
               RoleId = defaultRole.Id
           });

            if (!result.Success)
                return APIResponse<AuthorizationModel>.FailureResponse(result.Errors);

            var tokenModel = TokenMaker.CreateToken(checkPersonResult.Data.First(), new TokenDescriptorModel
            {
                Key = Convert.ToString(_config["Jwt:Key"]),
                ExpiresInMinutes = Convert.ToInt32(_config["Jwt:ExpiresInMinutes"]),
            });

            return APIResponse<AuthorizationModel>.SuccessResponse(new AuthorizationModel
            {
                PersonId = checkPersonResult.Data.First().Id.ToString(),
                KeepAuthorized = model.KeepAuthorized,
                Token = tokenModel.Token,
                TokenExpirationDate = tokenModel.ValidTo
            });
        }

        public async Task<IResponse<AuthorizationModel>> SignInAsync(SignInModel model)
        {
            var checkPersonResult = await _personRepo.GetAllAsync(x => x.Username == model.UserIdentifier || x.Email == model.UserIdentifier, null);

            if (checkPersonResult.Success && !checkPersonResult.Data.Any())
                return APIResponse<AuthorizationModel>.FailureResponse("Person with that email or username already signed up.");
            else if (!checkPersonResult.Success)
                return APIResponse<AuthorizationModel>.FailureResponse(checkPersonResult.Errors);

            if(!PassHandler.VerifyPassword(model.Password, checkPersonResult.Data.First().PasswordHash))
                return APIResponse<AuthorizationModel>.FailureResponse("Invalid user identificator or password.");

            var tokenModel = TokenMaker.CreateToken(checkPersonResult.Data.First(), new TokenDescriptorModel
            {
                Key = Convert.ToString(_config["Jwt:Key"]),
                ExpiresInMinutes = Convert.ToInt32(_config["Jwt:ExpiresInMinutes"]),
            });

            return APIResponse<AuthorizationModel>.SuccessResponse(new AuthorizationModel
            {
                PersonId = checkPersonResult.Data.First().Id.ToString(),
                KeepAuthorized = model.KeepAuthorized,
                Token = tokenModel.Token,
                TokenExpirationDate = tokenModel.ValidTo
            });
        }
    }
}
