using Microsoft.AspNetCore.Mvc.RazorPages;
using New_School_Management_API.PagInated_Response.QueryingDB;

namespace New_School_Management_API.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<PageResult<TResult>> PageResult<TResult>(QueriableParameter queriableParameter);

        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}
