using System.Collections.Generic;

namespace AsbaBank.Infrastructure
{
    public interface IRepository
    {
        IEnumerable<TPersistable> All<TPersistable>() where TPersistable : class;
        TPersistable Get<TPersistable>(object id) where TPersistable : class;
        TPersistable Add<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Remove<TPersistable>(TPersistable item) where TPersistable : class;
        TPersistable Update<TPersistable>(object id, TPersistable item) where TPersistable : class;
    }
}