using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.GenericRepository
{
    public class Repository<TEntity, TContext> : IRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        private readonly TContext _context;

        private DbSet<TEntity> Entity;

        public Repository(TContext context)
        {
            _context = context;
            Entity = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            Entity.Add(entity);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Entity.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Entity.AddRangeAsync(entities, cancellationToken);
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Entity.AnyAsync(expression, cancellationToken);
        }

        public void Delete(TEntity entity)
        {
            Entity.Remove(entity);
        }

        public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? default!;
            Entity.Remove(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            TEntity entity = await Entity.FindAsync(id) ?? default!;
            Entity.Remove(entity);
        }

        public void DeleteRange(ICollection<TEntity> entities)
        {
            Entity.RemoveRange(entities);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Entity.AsNoTracking().AsQueryable();
        }

        public IQueryable<TEntity> GetAllWithTracking()
        {
            return Entity.AsQueryable();
        }

        public TEntity GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).AsNoTracking().FirstOrDefault()!;
        }

        public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken)
                   ?? default!;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken), bool isTrackingActive = true)
        {
            return (!isTrackingActive) ? (await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken))! : (await Entity.Where(expression).FirstOrDefaultAsync(cancellationToken))!;
        }

        public TEntity GetByExpressionWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).FirstOrDefault()!;
        }

        public async Task<TEntity> GetByExpressionWithTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Entity.Where(expression).FirstOrDefaultAsync(cancellationToken) ?? default!;
        }

        public TEntity GetFirst()
        {
            return Entity.AsNoTracking().FirstOrDefault()!;
        }

        public async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Entity.AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? default!;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.AsNoTracking().Where(expression).AsQueryable();
        }

        public IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).AsQueryable();
        }

        public void Update(TEntity entity)
        {
            Entity.Update(entity);
        }

        public void UpdateRange(ICollection<TEntity> entities)
        {
            Entity.UpdateRange(entities);
        }

        public void AddRange(ICollection<TEntity> entities)
        {
            Entity.AddRange(entities);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
        {
            if (isTrackingActive)
            {
                return Entity.FirstOrDefault(expression) ?? default!;
            }

            return Entity.AsNoTracking().FirstOrDefault(expression) ?? default!;
        }

        public IQueryable<KeyValuePair<bool, int>> CountBy(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entity.CountBy(expression);
        }

        public TEntity First(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
        {
            if (isTrackingActive)
            {
                return Entity.First(expression);
            }

            return Entity.AsNoTracking().First(expression);
        }

        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default(CancellationToken), bool isTrackingActive = true)
        {
            if (isTrackingActive)
            {
                return await Entity.FirstAsync(expression, cancellationToken);
            }

            return await Entity.AsNoTracking().FirstAsync(expression, cancellationToken);
        }
    }
}
