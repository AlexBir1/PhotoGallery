using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.BaseResponse;
using PhotoGalleryAPI.BaseResponse.Responses;
using PhotoGalleryAPI.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IResponse<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> exp = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = exp != null ? _dbSet.AsQueryable().Where(exp) : _dbSet.AsQueryable();
            if (include != null)
            {
                query = include(query);
            }

            return DbResponse<IEnumerable<T>>.SuccessResponse(await query.AsNoTracking().ToListAsync());
        }

        public async Task<IResponse<T>> GetByIdAsync(Guid id)
        {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return DbResponse<T>.FailureResponse("No data found. Nothing to view or operate with.");
                }
                return DbResponse<T>.SuccessResponse(entity);
        }

        public async Task<IResponse<T>> AddAsync(T entity)
        {
                var result = await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return DbResponse<T>.SuccessResponse(result.Entity);
        }

        public async Task<IResponse<T>> UpdateAsync(T entity)
        {
                var result = _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return DbResponse<T>.SuccessResponse(result.Entity);
        }

        public async Task<IResponse<T>> DeleteAsync(Guid id)
        {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return DbResponse<T>.FailureResponse("No data found. Nothing to view or operate with.");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return DbResponse<T>.SuccessResponse(entity);
        }

        public async Task<IResponse<IEnumerable<T>>> GetAllAsync(int itemsPerPage = 1, int selectedPage = 1, Expression<Func<T, bool>> exp = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            int countToSkip = selectedPage > 1 ? (selectedPage - 1) * itemsPerPage : 0;

            var query = exp != null ? _dbSet.AsQueryable().Where(exp) : _dbSet.AsQueryable();
            if (include != null)
            {
                query = include(query);
            }
            return DbResponse<IEnumerable<T>>.SuccessResponse(await query.Skip(countToSkip).Take(itemsPerPage).AsNoTracking().ToListAsync());
        }
    }
}
