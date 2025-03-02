using AutoMapper;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.Repositories;
using PhotoGalleryAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Services.Services
{
    public abstract class BaseService<TEntity, TDto, TKey> : IService<TEntity, TDto, TKey>
    where TEntity : class
    where TDto : class
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IResponse<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>> exp = null)
        {
            var response = exp != null ? await _repository.GetAllAsync(exp) : await _repository.GetAllAsync(null);
            if (!response.Success)
            {
                return APIResponse<IEnumerable<TDto>>.FailureResponse(response.Errors);
            }

            var dtos = _mapper.Map<IEnumerable<TDto>>(response.Data);
            return APIResponse<IEnumerable<TDto>>.SuccessResponse(dtos);
        }

        public virtual async Task<IResponse<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>> exp = null, int itemsPerPage = 1, int selectedPage = 1)
        {
            var response = exp != null ? 
                await _repository.GetAllAsync(itemsPerPage, selectedPage, exp) as DbResponse<IEnumerable<TEntity>>: 
                await _repository.GetAllAsync(itemsPerPage, selectedPage) as DbResponse<IEnumerable<TEntity>>;
            if (!response.Success)
            {
                return APIResponse<IEnumerable<TDto>>.FailureResponse(response.Errors);
            }

            var dtos = _mapper.Map<IEnumerable<TDto>>(response.Data);
            return APIResponse<IEnumerable<TDto>>.SuccessPagedResponse(dtos,itemsPerPage,selectedPage, response.ItemsCount);
        }

        public virtual async Task<IResponse<TDto>> GetByIdAsync(TKey id)
        {
            var response = await _repository.GetByIdAsync(Guid.Parse(id.ToString()));
            if (!response.Success)
            {
                return APIResponse<TDto>.FailureResponse(response.Errors);
            }

            var dto = _mapper.Map<TDto>(response.Data);
            return APIResponse<TDto>.SuccessResponse(dto);
        }

        public virtual async Task<IResponse<TDto>> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var response = await _repository.AddAsync(entity);
            if (!response.Success)
            {
                return APIResponse<TDto>.FailureResponse(response.Errors);
            }

            var result = _mapper.Map<TDto>(response.Data);
            return APIResponse<TDto>.SuccessResponse(result);
        }

        public virtual async Task<IResponse<TDto>> UpdateAsync(TKey id, TDto dto)
        {
            var response = await _repository.GetByIdAsync(Guid.Parse(id.ToString()));
            if (!response.Success)
            {
                return APIResponse<TDto>.FailureResponse(response.Errors);
            }

            var entity = response.Data;
            _mapper.Map(dto, entity);

            var updateResponse = await _repository.UpdateAsync(entity);
            if (!updateResponse.Success)
            {
                return APIResponse<TDto>.FailureResponse(updateResponse.Errors);
            }

            var result = _mapper.Map<TDto>(updateResponse.Data);
            return APIResponse<TDto>.SuccessResponse(result);
        }

        public virtual async Task<IResponse<TDto>> DeleteAsync(TKey id)
        {
            var response = await _repository.DeleteAsync(Guid.Parse(id.ToString()));
            if (!response.Success)
            {
                return APIResponse<TDto>.FailureResponse(response.Errors);
            }

            var result = _mapper.Map<TDto>(response.Data);
            return APIResponse<TDto>.SuccessResponse(result);
        }
    }
}
