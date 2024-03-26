using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TentHouseRental.BussinessLogic;
using TentHouseRental.Models;

namespace TentHouseRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ITentHouseRentalService houseRentalService;

        public CustomerController(ITentHouseRentalService houseRentalService)
        {
            this.houseRentalService = houseRentalService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCustomer()
        {
            var customerList = await houseRentalService.GetAllCustomer();
            return Ok(customerList);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(CustomerModel customer)
        {
            var response = await houseRentalService.AddCustomer(customer);
            return Ok(response);
        }

    }
}
