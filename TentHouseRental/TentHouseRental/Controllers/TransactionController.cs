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
    public class TransactionController : ControllerBase
    {
        private readonly ITentHouseRentalService tentHouseRentalService;

        public TransactionController(ITentHouseRentalService houseRentalService)
        {
            this.tentHouseRentalService = houseRentalService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTransaction()
        {
            var transactionList = await tentHouseRentalService.GetAllTransaction();
            return Ok(transactionList);
        }

        [HttpPost]
        public async Task<ActionResult> AddTransaction(TransactionHistoryModel transaction)
        {
            var response = await tentHouseRentalService.AddTransaction(transaction);
            return Ok(new { success = response.Item1, transactionId = response.Item2 });
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllTransaction()
        {
            var response = await tentHouseRentalService.DeleteAllTransactions();
            return Ok(response);
        }

    }
}
