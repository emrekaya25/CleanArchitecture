using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Common.Repositories
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

        /// <summary>
        /// Belirtilen entity'yi veritabanına ekler.
        /// </summary>
        public void Add(TEntity entity)
        {
            Entity.Add(entity);
        }

        /// <summary>
        /// Belirtilen entity'yi veritabanına asenkron olarak ekler.
        /// </summary>
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Entity.AddAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Bir koleksiyon içindeki entity'leri asenkron olarak ekler.
        /// </summary>
        public async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await Entity.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Belirtilen şarta uyan herhangi bir kayıt olup olmadığını kontrol eder.
        /// </summary>
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Any(expression);
        }

        /// <summary>
        /// Belirtilen şarta uyan herhangi bir kayıt olup olmadığını asenkron olarak kontrol eder.
        /// </summary>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Entity.AnyAsync(expression, cancellationToken);
        }

        /// <summary>
        /// Verilen entity'yi veritabanından siler.
        /// </summary>
        public void Delete(TEntity entity)
        {
            Entity.Remove(entity);
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi asenkron olarak bulup siler.
        /// </summary>
        public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? default!;
            Entity.Remove(entity);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip entity'yi asenkron olarak bulup siler.
        /// </summary>
        public async Task DeleteByIdAsync(string id)
        {
            TEntity entity = await Entity.FindAsync(id) ?? default!;
            Entity.Remove(entity);
        }

        /// <summary>
        /// Bir koleksiyon içindeki entity'leri toplu olarak siler.
        /// </summary>
        public void DeleteRange(ICollection<TEntity> entities)
        {
            Entity.RemoveRange(entities);
        }

        /// <summary>
        /// Veritabanındaki tüm entity'leri getirir (Tracking devre dışı).
        /// </summary>
        public IQueryable<TEntity> GetAll()
        {
            return Entity.AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Veritabanındaki tüm entity'leri getirir (Tracking açık).
        /// </summary>
        public IQueryable<TEntity> GetAllWithTracking()
        {
            return Entity.AsQueryable();
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi getirir (Tracking devre dışı).
        /// </summary>
        public TEntity GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).AsNoTracking().FirstOrDefault()!;
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi asenkron olarak getirir (Tracking devre dışı).
        /// </summary>
        public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? default!;
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi getirir (Tracking tercihe bağlı).
        /// </summary>
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true)
        {
            return !isTrackingActive ? (await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken))! : (await Entity.Where(expression).FirstOrDefaultAsync(cancellationToken))!;
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi getirir (Tracking açık).
        /// </summary>
        public TEntity GetByExpressionWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).FirstOrDefault()!;
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi asenkron olarak getirir (Tracking açık).
        /// </summary>
        public async Task<TEntity> GetByExpressionWithTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Entity.Where(expression).FirstOrDefaultAsync(cancellationToken) ?? default!;
        }

        /// <summary>
        /// Veritabanındaki ilk entity'yi getirir (Tracking devre dışı).
        /// </summary>
        public TEntity GetFirst()
        {
            return Entity.AsNoTracking().FirstOrDefault()!;
        }

        /// <summary>
        /// Veritabanındaki ilk entity'yi asenkron olarak getirir (Tracking devre dışı).
        /// </summary>
        public async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default)
        {
            return await Entity.AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? default!;
        }

        /// <summary>
        /// Belirtilen şarta uyan entity'leri getirir (Tracking devre dışı).
        /// </summary>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.AsNoTracking().Where(expression).AsQueryable();
        }

        /// <summary>
        /// Belirtilen şarta uyan entity'leri getirir (Tracking açık).
        /// </summary>
        public IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            return Entity.Where(expression).AsQueryable();
        }

        /// <summary>
        /// Belirtilen entity'yi günceller.
        /// </summary>
        public void Update(TEntity entity)
        {
            Entity.Update(entity);
        }

        /// <summary>
        /// Bir koleksiyon içindeki entity'leri toplu olarak günceller.
        /// </summary>
        public void UpdateRange(ICollection<TEntity> entities)
        {
            Entity.UpdateRange(entities);
        }

        /// <summary>
        /// Bir koleksiyon içindeki entity'leri toplu olarak ekler.
        /// </summary>
        public void AddRange(ICollection<TEntity> entities)
        {
            Entity.AddRange(entities);
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi getirir (Tracking tercihe bağlı).
        /// </summary>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
        {
            return isTrackingActive ? Entity.FirstOrDefault(expression) ?? default! : Entity.AsNoTracking().FirstOrDefault(expression) ?? default!;
        }

        /// <summary>
        /// Belirtilen şarta göre entity sayısını döndürür.
        /// </summary>
        public IQueryable<KeyValuePair<bool, int>> CountBy(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return Entity.CountBy(expression);
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi getirir (Tracking tercihe bağlı).
        /// </summary>
        public TEntity First(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
        {
            return isTrackingActive ? Entity.First(expression) : Entity.AsNoTracking().First(expression);
        }

        /// <summary>
        /// Belirtilen şarta uyan ilk entity'yi asenkron olarak getirir (Tracking tercihe bağlı).
        /// </summary>
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true)
        {
            if (isTrackingActive)
            {
                return await Entity.FirstAsync(expression, cancellationToken);
            }

            return await Entity.AsNoTracking().FirstAsync(expression, cancellationToken);
        }
    }
}
