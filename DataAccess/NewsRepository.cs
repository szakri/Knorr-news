using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataAccess
{
    public class NewsRepository : INewsRepository
    {
        private readonly KnorrNewsContext _dbContext;

        public NewsRepository(KnorrNewsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<News>> GetListAsync()
        {
            return await _dbContext.News.Include(n => n.Links)
                .OrderByDescending(n => n.PublishDate)
                .ToListAsync();
        }

        public IQueryable<News> GetQueryable()
        {
            return _dbContext.News.Include(n => n.Links);
        }

        public async Task<News> GetAsync(int id)
        {
            var entity = await _dbContext.News.Include(n => n.Links)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (entity == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }
            return entity;
        }

        public async Task<bool> HasNewsAsync(string newsTitle)
        {
            return await _dbContext.News.AnyAsync(n => n.Title == newsTitle);
        }

        public async Task AddAsync(News entity)
        {
            await _dbContext.News.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<News> entity)
        {
            await _dbContext.News.AddRangeAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(News entity)
        {
            _dbContext.News.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(News entity)
        {
            _dbContext.News.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
