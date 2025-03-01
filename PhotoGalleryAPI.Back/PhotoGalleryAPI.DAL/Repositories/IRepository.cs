using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IResponse<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> exp = null, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<IResponse<IEnumerable<T>>> GetAllAsync(int itemsPerPage = 1, int selectedPage = 1, Expression<Func<T, bool>> exp = null, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<IResponse<T>> GetByIdAsync(Guid id);
        Task<IResponse<T>> AddAsync(T entity);
        Task<IResponse<T>> UpdateAsync(T entity);
        Task<IResponse<T>> DeleteAsync(Guid id);
    }
}
