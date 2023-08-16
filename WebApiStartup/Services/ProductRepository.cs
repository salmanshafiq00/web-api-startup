using Microsoft.EntityFrameworkCore;
using WebApiStartup.Data;
using WebApiStartup.Entity;
using WebApiStartup.Interfaces;

namespace WebApiStartup.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return null;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<List<Product>> GetAll() => await _context.Products.ToListAsync();

        public async Task<Product?> GetById(int id) => await _context.Products.FindAsync(id);

        public async Task<Product?> Update(Product product)
        {
            var existProduct = await _context.Products.FindAsync(product.Id);
            if (existProduct is null)
            {
                return null;
            }
            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.PhotoUrl = product.PhotoUrl;

            await _context.SaveChangesAsync();

            return existProduct;
        }
    }
}
