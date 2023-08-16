using Microsoft.EntityFrameworkCore;
using WebApiStartup.Data;
using WebApiStartup.Entity;
using WebApiStartup.Interfaces;

namespace WebApiStartup.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> Create(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category is null)
            {
                return null;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<List<Category>> GetAll() => await _context.Categories.ToListAsync();

        public async Task<Category?> GetById(int id)  => await _context.Categories.FindAsync(id);

        public async Task<Category?> Update(Category category)
        {
            var existCategory = await _context.Categories.FindAsync(category.Id);
            if(existCategory is null)
            {
                return null;
            }
            existCategory.Name = category.Name;
            existCategory.Description = category.Description;
            existCategory.PhotoUrl = category.PhotoUrl;

            await _context.SaveChangesAsync();

            return existCategory;
        }
    }
}
