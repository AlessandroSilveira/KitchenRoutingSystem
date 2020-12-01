using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Repositories
{
    public interface IRepository<T> 
    {
        Task<List<T>> GetAll();

        Task<T> Get(string id);

        Task<T> Add(T item);

        Task Edit(T item);

        Task Delete(string id);
    }
}