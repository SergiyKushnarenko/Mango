using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Services.ProductAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _contex;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext contex, IMapper mapper)
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto prodict)
        {
            var product = _mapper.Map<Product>(prodict);
            if (product.ProductId > 0)
            {
                _contex.Products.Update(product);
            }
            else
            {
                _contex.Products.Add(product);
            }
            await _contex.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int prodictId)
        {
            try
            {
                var product = await _contex.Products.FirstOrDefaultAsync(e => e.ProductId == prodictId);
                if (product is null) return false;
                _contex.Products.Remove(product);
                await _contex.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            List<Product> products = await _contex.Products.ToListAsync();
            var result = _mapper.Map<List<ProductDto>>(products);
            return result;

        }

        public async Task<ProductDto> GetProductById(int prodictId)
        {
            var product = await _contex.Products
                .Where(e => e.ProductId == prodictId)
                .FirstOrDefaultAsync(e => e.ProductId == prodictId);
            var result = _mapper.Map<ProductDto>(product);
            return result;
        }
    }
}
