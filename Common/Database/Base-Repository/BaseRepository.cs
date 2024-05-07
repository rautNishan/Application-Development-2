using System.Linq.Expressions;
using CourseWork.Common.Constants.Enums;
using CourseWork.Common.database.Base_Model;
using CourseWork.Common.database.Interfaces;
using CourseWork.Common.Database.Interfaces;
using CourseWork.Common.Middlewares.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CourseWork.Common.database.Base_Repository
{
    public class BaseRepository<T> : IDataBaseBaseInterface<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;


        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        //CRUD
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<PaginatedResponse<T>> GetAllPaginatedAsync(int pageNumber, ShortByEnum shortBy)
        {
            int dataPerPage = 20; //Get through enum or constant
            var totalCount = await _dbSet.CountAsync();
            IQueryable<T> sortedData;


            if (shortBy == ShortByEnum.Latest)
            {
                sortedData = _dbSet.OrderByDescending(entity => entity.CreatedAt);
            }
            else // SortOrder.Oldest
            {
                sortedData = _dbSet.OrderBy(entity => entity.CreatedAt);
            }

            IEnumerable<T> data = await sortedData.Skip((pageNumber - 1) * dataPerPage).Take(dataPerPage).ToListAsync();
            return new PaginatedResponse<T>
            {
                PageNumber = pageNumber,
                DataPerPage = dataPerPage,
                TotalCount = totalCount,
                Data = data,
            };

        }

        public async Task<T?> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> FindOne(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> CreateAsync(T entity, bool UseTransaction = false)
        {
            if (UseTransaction)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _dbSet.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return entity;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<T> UpdateAsync(T entity, bool useTransaction = false)
        {
            if (useTransaction)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _dbSet.Update(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return entity;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
        public async Task<T> DeleteAsync(T entity, bool useTransaction = false)
        {
            if (useTransaction)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return entity;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<T> SoftDeleteAsync(T entity, bool useTransaction = false)
        {
            if (useTransaction)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();

            }
            return entity;
        }

    }
}
