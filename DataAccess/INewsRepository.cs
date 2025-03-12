using Models.Models;

namespace DataAccess
{
    public interface INewsRepository
    {
        public Task<List<News>> GetListAsync();

        public IQueryable<News> GetQueryable();

        public Task<News> GetAsync(int id);

        public Task<bool> HasNewsAsync(string newsTitle);

        public Task AddAsync(News entity);

        public Task AddRangeAsync(IEnumerable<News> entity);

        public void Update(News entity);

        public void Delete(News entity);
    }
}
