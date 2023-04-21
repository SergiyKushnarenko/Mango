using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Services.ProductAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto responseDto;
        private readonly IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this.responseDto = new ResponseDto();
        }

        
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _productRepository.GetProducts();
                responseDto.Result = productDtos;
               
            }
            catch (Exception ex)
            {
                responseDto.ErrorMessages = new List<string>() {ex.ToString()};
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpGet]
		[Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductDto productDto = await _productRepository.GetProductById(id);
                responseDto.Result = productDto;

            }
            catch (Exception ex)
            {
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpPost]
		[Authorize]
		public async Task<object> Post([FromBody]ProductDto productDto)
        {
            try
            {
                ProductDto productDtoModel = await _productRepository.CreateUpdateProduct(productDto);
                responseDto.Result = productDtoModel;

            }
            catch (Exception ex)
            {
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpPut]
		[Authorize]
		public async Task<object> Put([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto productDtoModel = await _productRepository.CreateUpdateProduct(productDto);
                responseDto.Result = productDtoModel;

            }
            catch (Exception ex)
            {
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }

        [HttpDelete]
		[Authorize(Roles ="Admin")]
		[Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                var IsSuccess = await _productRepository.DeleteProduct(id);
                responseDto.Result = IsSuccess;

            }
            catch (Exception ex)
            {
                responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                responseDto.IsSuccess = false;
            }
            return responseDto;
        }
    }
}
