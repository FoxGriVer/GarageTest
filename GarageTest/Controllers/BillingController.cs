using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageTest.Entities;
using GarageTest.Entities.DbContext;
using Microsoft.EntityFrameworkCore;

namespace GarageTest.Controllers
{
    /// <summary>
    ///     The controller is responsible for logging and reporting
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : Controller
    {
        GarageDbContext dbContext;

        public BillingController(GarageDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        ///     Creating a report
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateReport")]
        public async Task<ActionResult<Report>> CreateReport()
        {
            List<ParkingSpaceBill> parkingSpaceBills = await dbContext.ParkingSpaceBills.Include(t => t.ParkingSpaceType).ToListAsync();
            List<ParkingSpaceType> parkingTypes = await dbContext.ParkingSpaceTypes.ToListAsync();
            ParkingGarage parkingSpacesInfo = await dbContext.ParkingGarages.FirstOrDefaultAsync();

            Report report = new Report();


            foreach (ParkingSpaceType parkingType in parkingTypes)
            {
                report.AvailableParkingSpaces += parkingType.CurrentAvalibleNumberSpots;
                report.PercentageOfParkingSpacesUsedByParkingType.Add(parkingType.ParkingSpaceName,
                    100 - (parkingType.CurrentAvalibleNumberSpots * 100 / parkingType.MaximumNumberSpots));
            }

            foreach (ParkingSpaceBill parkingSpaceBill in parkingSpaceBills)
            {
                if (parkingSpaceBill.Paid is true)
                {
                    report.HistoricalOverallProfit += parkingSpaceBill.TotalAmount;
                }
                else
                {
                    report.OutstandingProfit += parkingSpaceBill.TimeSpentInTheParking.Hours * parkingSpaceBill.ParkingSpaceType.PricePerHour;
                }
            }

            report.PercentageOfParkingSpacesUsed = 100 - (report.AvailableParkingSpaces * 100 / parkingSpacesInfo.LimitedAmount);

            return new ObjectResult(report);
        }
    }
}
