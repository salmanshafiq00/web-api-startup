using WebApiStartup.Entity;

namespace WebApiStartup.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task<Category> Delete(int id);
    }
}
