using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public interface IRepository<TEntity, in TIdentity>
        where TEntity : IEntity<TIdentity>
    {
        TEntity GetById(TIdentity id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TIdentity id);
    }
}