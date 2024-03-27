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
        private readonly ITentHouseRentalService tentHouseRentalService;

        public CustomerController(ITentHouseRentalService tentHouseRentalService)
        {
            this.tentHouseRentalService = tentHouseRentalService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCustomer()
        {
            var customerList = await tentHouseRentalService.GetAllCustomer();
            return Ok(customerList);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(CustomerModel customer)
        {
            var response = await tentHouseRentalService.AddCustomer(customer);
            return Ok(response);
        }

    }
}
