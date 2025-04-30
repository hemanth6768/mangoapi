using MangoApi.MangoModels;

namespace MangoApi.MangoRepositoiry
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(string id, Product product);
        Task DeleteProductAsync(string id);
    }
}
