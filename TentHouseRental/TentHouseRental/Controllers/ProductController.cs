using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TentHouseRental.BussinessLogic;
using TentHouseRental.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TentHouseRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ITentHouseRentalService tentHouseRentalService;

        public ProductController(ITentHouseRentalService tentHouseRentalService)
        {
            this.tentHouseRentalService = tentHouseRentalService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProduct()
        {
            var productList = await tentHouseRentalService.GetAllProduct();
            return Ok(productList);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult> GetProductById(int productId)
        {
            var product = await tentHouseRentalService.GetProductById(productId);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductModel product)
        {
            var response = await tentHouseRentalService.AddProduct(product);
            return Ok(response);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(int productId, ProductModel product)
        {
            var response = await tentHouseRentalService.UpdateProduct(productId, product);
            return Ok(response);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var response = await tentHouseRentalService.RemoveProduct(productId);
            return Ok(response);
        }

    }
}
