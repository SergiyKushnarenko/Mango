using Mango.Services.ProductAPI.Models.DTOs;

namespace Mango.Services.ProductAPI.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int prodictId);
        Task<ProductDto> CreateUpdateProduct(ProductDto prodict); 
        Task<bool> DeleteProduct(int prodictId);
    }
}
