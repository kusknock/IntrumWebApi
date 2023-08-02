using IntrumWebApi.Exceptions;
using IntrumWebApi.Models;
using IntrumWebApi.Services.Other.Interfaces;
using ItrumWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace IntrumWebApi.Services.Other
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext context;

        public ProductService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Product> Create([Required] Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            var result = await context.Products.AddAsync(product);

            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Product> Delete(int id)
        {
            var productItem = await context.Products.FindAsync(id);

            if (productItem == null)
                throw new ItemNotFoundException("Item was not found in database");

            var result = context.Products.Remove(productItem);

            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Product> Edit(int id, [Required] Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            var productItem = await context.Products.FindAsync(id);

            if (productItem is null)
                throw new ItemNotFoundException("Item was not found in database");

            productItem.Name = product.Name;
            productItem.Description = product.Description;
            productItem.ImageId = product.ImageId;
            productItem.Price = product.Price;

            await context.SaveChangesAsync();

            return productItem;
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public async Task<Product> GetById(int id)
        {
            var productItem = await context.Products.FindAsync(id);

            if (productItem is null)
                throw new ItemNotFoundException("Item was not found in database");

            return productItem;
        }
    }
}
