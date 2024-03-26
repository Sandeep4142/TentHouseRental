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
    public class Reports : ControllerBase
    {
        private readonly ITentHouseRentalService houseRentalService;

        public Reports(ITentHouseRentalService houseRentalService)
        {
            this.houseRentalService = houseRentalService;
        }

        [HttpGet("InventorySummaryReport")]
        public async Task<ActionResult> GetInventorySummaryReport()
        {
            var productList = await houseRentalService.GetAllProduct();
            return Ok(productList);
        }

        [HttpGet("InventoryDetailedReport")]
        public async Task<ActionResult> GetInventoryDetailedReport()
        {
            var detailedReport = await houseRentalService.GetInventoryDetailedReport();
            return Ok(detailedReport);
        }

        [HttpPost("InventoryDetailedReportByDate/{date}")]
        public async Task<ActionResult> GetInventoryDetailedReportByDate(DateTime date)
        {
            var detailedReport = await houseRentalService.GetInventoryDetailedReportByDate(date);
            return Ok(detailedReport);
        }

        [HttpPost("InventoryDetailedReportByMonth/{month}")]
        public async Task<ActionResult> GetInventoryDetailedReportByMonth(DateTime month)
        {
            var detailedReport = await houseRentalService.GetInventoryDetailedReportByMonth(month);
            return Ok(detailedReport);
        }

    }
}
