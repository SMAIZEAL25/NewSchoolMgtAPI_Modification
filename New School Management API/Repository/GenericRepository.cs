using Microsoft.EntityFrameworkCore;
using New_School_Management_API.Dbcontext;
using New_School_Management_API.QueryingDB;

namespace New_School_Management_API.Repository
{
    public class GenericRepository <T>: IGenericRepository <T> where T : class
    {
        private readonly StudentManagementDB _studentManagementDB;

        public GenericRepository(StudentManagementDB studentManagementDB)
        {
            this._studentManagementDB = studentManagementDB;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _studentManagementDB.AddAsync(entity);
            await _studentManagementDB.SaveChangesAsync();
            return entity;

        }

   

        public async Task DeleteAsync(int Id)
        {
            var entity = await GetByIdAsync(Id);
            _studentManagementDB.Set<T>().Remove(entity);
            await _studentManagementDB.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }


        public async Task<List<T>> GetAllAsync()
        {
            var response = await _studentManagementDB.Set<T>().ToListAsync();
            return response;
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await _studentManagementDB.Set<T>().FindAsync(id);
        }

        public Task<PageResult<TResult>> PageResult<TResult>(QueriableParameter queriableParameter)
        {
            //var totalSize = await _context.Set<T>().CountAsync();
            //var items = await _context.Set<T>()
            //    .Skip(queryParameters.StartIndex)
            //    .Take(queryParameters.PageSize)

            //    // first inject the mapper 
            //    .ProjectTo<TResult>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            //return new PageResult<TResult>
            //{
            //    Items = items,
            //    PageNumber = queryParameters.StartIndex,
            //    RecordNumber = queryParameters.PageSize,
            //    TotalCount = totalSize

            //};
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(T entity)
        {
             _studentManagementDB.Update(entity);
             await _studentManagementDB.SaveChangesAsync();
        }

       
    }

    
}
