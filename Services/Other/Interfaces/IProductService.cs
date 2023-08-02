using IntrumWebApi.Models;

namespace IntrumWebApi.Services.Other.Interfaces
{
    public interface IProductService
    {
        Task<Product> Create(Product product);
        Task<Product> Edit(int id, Product product);
        Task<Product> Delete(int id);
        Task<Product> GetById(int id);
        IEnumerable<Product> GetAll();

    }
}
