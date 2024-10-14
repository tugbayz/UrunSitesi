using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using PackProApp.Core.BaseEntities;
using PackProApp.DataAccess.Interfaces;
using System.Linq.Expressions;
using PackProApp.Enums;

namespace PackProApp.DataAccess.EntityFramework
{
    public class EFBaseRepository<TEntity> : IRepository, IAsyncRepository, IAsyncUpdatableRepository<TEntity>,
        IAsyncInsertableRepository<TEntity>, IAsyncDeletableRepository<TEntity>, IAsyncFindableRepository<TEntity>,
        IAsyncQueryableRepository<TEntity>, IAsyncOrderableRepository<TEntity>, IAsyncTransactionRepository where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _table;
        public EFBaseRepository(DbContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _table.AddAsync(entity);
            return entry.Entity;


        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression is null ? GetAllActives().AnyAsync() : GetAllActives().AnyAsync(expression);

        }



        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task<IExecutionStrategy> CreateExecutionStrategy()
        {
            return Task.FromResult(_context.Database.CreateExecutionStrategy());
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.FromResult(_table.Remove(entity));
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true)
        {
            return await GetAllActives(tracking).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            return await GetAllActives(tracking).Where(expression).ToListAsync();
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool orderByDesc, bool tracking = true)
        {
            return orderByDesc ? await GetAllActives(tracking).OrderByDescending(orderBy).ToListAsync() : await
                GetAllActives(tracking).OrderBy(orderBy).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, bool orderByDesc, bool tracking = true)
        {
            var values = GetAllActives(tracking).Where(expression);
            return orderByDesc ? await values.OrderByDescending(orderBy).ToListAsync() : await
                values.OrderBy(orderBy).ToListAsync();
        }


        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(x => x.Id == id);
        }

        public int SaveChange()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }



        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.FromResult(_table.Update(entity).Entity);
        }

        protected IQueryable<TEntity> GetAllActives(bool tracking = true)
        {
            var values = _table.Where(x => x.Status != Status.Deleted);
            return tracking ? values : values.AsNoTracking();
        }
    }
}
