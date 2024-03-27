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
        private readonly ITentHouseRentalService tentHouseRentalService;

        public Reports(ITentHouseRentalService tentHouseRentalService)
        {
            this.tentHouseRentalService = tentHouseRentalService;
        }

        [HttpGet("InventorySummaryReport")]
        public async Task<ActionResult> GetInventorySummaryReport()
        {
            var productList = await tentHouseRentalService.GetAllProduct();
            return Ok(productList);
        }

        [HttpGet("InventoryDetailedReport")]
        public async Task<ActionResult> GetInventoryDetailedReport()
        {
            var detailedReport = await tentHouseRentalService.GetInventoryDetailedReport();
            return Ok(detailedReport);
        }

        [HttpPost("InventoryDetailedReportByDate/{date}")]
        public async Task<ActionResult> GetInventoryDetailedReportByDate(DateTime date)
        {
            var detailedReport = await tentHouseRentalService.GetInventoryDetailedReportByDate(date);
            return Ok(detailedReport);
        }

        [HttpPost("InventoryDetailedReportByMonth/{month}")]
        public async Task<ActionResult> GetInventoryDetailedReportByMonth(DateTime month)
        {
            var detailedReport = await tentHouseRentalService.GetInventoryDetailedReportByMonth(month);
            return Ok(detailedReport);
        }

    }
}
