using MangoApi.MangoModels;
using MangoApi.MangoRepositoiry;

namespace MangoApi.MangoService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return _productRepository.GetAllProductsAsync();
        }

        public Task<Product> GetProductByIdAsync(string id)
        {
            return _productRepository.GetProductByIdAsync(id);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            return _productRepository.CreateProductAsync(product);
        }

        public Task<Product> UpdateProductAsync(string id, Product product)
        {
            return _productRepository.UpdateProductAsync(id, product);
        }

        public Task DeleteProductAsync(string id)
        {
            return _productRepository.DeleteProductAsync(id);
        }
    }
}

