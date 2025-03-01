using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.Services.Interfaces
{
    public interface IService<TEntity, TDto, TKey> where TDto : class
    {
        Task<IResponse<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>> exp = null);
        Task<IResponse<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>> exp = null, int itemsPerPage = 1, int selectedPage = 1);
        Task<IResponse<TDto>> GetByIdAsync(TKey id);
        Task<IResponse<TDto>> AddAsync(TDto dto);
        Task<IResponse<TDto>> UpdateAsync(TKey id, TDto dto);
        Task<IResponse<TDto>> DeleteAsync(TKey id);
    }
}
