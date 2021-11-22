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
    ///     The controller is responsible for working with parking receipts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : Controller
    {
        GarageDbContext dbContext;

        public ParkingController(GarageDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        ///     The process when the car enters the parking lot
        /// </summary>
        /// <param name="parkingSpaceBill"></param>
        /// <returns></returns>
        [HttpPost("SetParkingSpace")]
        public async Task<ActionResult<ParkingSpaceBill>> CarGetIn(ParkingSpaceBill parkingSpaceBill)
        {
            if (parkingSpaceBill == null)
            {
                return BadRequest();
            }

            if (!CheckAvaliblePlaceOfTheType(parkingSpaceBill))
            {
                return Ok("There are no vacant seats of this type.");
            }
            else
            {
                parkingSpaceBill.TimeIn = Convert.ToDateTime(DateTime.Now);
                parkingSpaceBill.TimeOut = Convert.ToDateTime(DateTime.Now.AddHours(1));
                parkingSpaceBill.TimeSpentInTheParking = parkingSpaceBill.TimeOut.Subtract(parkingSpaceBill.TimeIn);
                parkingSpaceBill.Paid = false;

                dbContext.ParkingSpaceBills.Add(parkingSpaceBill);
                await dbContext.SaveChangesAsync();

                return Ok(parkingSpaceBill);
            }            
        }

        /// <summary>
        ///     The process when the car leaves the parking lot
        /// </summary>
        /// <param name="parkingSpaceBill"></param>
        /// <param name="numberOfStandingHours"></param>
        /// <returns></returns>
        [HttpPut("EndParkingSpace/{numberOfStandingHours}")]
        public async Task<ActionResult<ParkingSpaceBill>> CarGetOut(ParkingSpaceBill parkingSpaceBill, int numberOfStandingHours)
        {
            ParkingSpaceBill existingParkingSpaceBill = await dbContext.ParkingSpaceBills.Include(t => t.ParkingSpaceType).FirstOrDefaultAsync(x => x.Id == parkingSpaceBill.Id);

            if (parkingSpaceBill == null)
            {
                return BadRequest();
            }

            existingParkingSpaceBill.TimeOut = Convert.ToDateTime(DateTime.Now.AddHours(numberOfStandingHours));
            existingParkingSpaceBill.TotalAmount = numberOfStandingHours * existingParkingSpaceBill.ParkingSpaceType.PricePerHour;
            existingParkingSpaceBill.TimeSpentInTheParking = existingParkingSpaceBill.TimeOut.Subtract(existingParkingSpaceBill.TimeIn);
            existingParkingSpaceBill.Paid = true;

            ++existingParkingSpaceBill.ParkingSpaceType.CurrentAvalibleNumberSpots;

            dbContext.Update(existingParkingSpaceBill);
            await dbContext.SaveChangesAsync();

            return Ok(existingParkingSpaceBill);
        }

        /// <summary>
        ///     Checking the availability of free parking space for a specific type
        ///     
        ///     ToDo: It is better to transfer this method to the parking type controller, 
        ///     but it takes time to create a service.
        /// </summary>
        /// <param name="ParkingSpaceTypeId"></param>
        /// <returns></returns>
        public bool CheckAvaliblePlaceOfTheType(ParkingSpaceBill parkingSpaceBill)
        {
            List<ParkingSpaceType> parkingSpacesTypeInfo = dbContext.ParkingSpaceTypes.ToList();

            ParkingSpaceType parkingSpaceTypeInfo = parkingSpacesTypeInfo.FirstOrDefault(x => x.Id == parkingSpaceBill.ParkingSpaceTypeId);
            // ToDo: In the future, it is better to convert the names to Enum type                                         

            // We check the number of places for a specific type of parking
            if ((parkingSpaceTypeInfo.CurrentAvalibleNumberSpots - 1) >= 0)
            {
                // We check whether the car is registered for this parking lot type
                if (!parkingSpaceTypeInfo.ParkingSpaceName.Equals("NormalParking") ||
                    parkingSpaceBill.CarLicensePlateRegistered)
                {
                    --parkingSpaceTypeInfo.CurrentAvalibleNumberSpots;

                    dbContext.Update(parkingSpaceTypeInfo);
                    dbContext.SaveChanges();

                    return true;
                }
                // If this car is not registered or have already normal,
                // then it will have the opportunity to park in a normal parking lot.
                else
                {
                    return CheckAvaliblePlacesForNormalType();
                }
            }
            // If the current type has no available spaces,
            // we will check the available spaces in a normal parking lot.
            else
            {
                return CheckAvaliblePlacesForNormalType();
            }
        }

        public bool CheckAvaliblePlacesForNormalType()
        {
            List<ParkingSpaceType> parkingSpacesTypeInfo = dbContext.ParkingSpaceTypes.ToList();

            ParkingSpaceType normalParkingSpaceTypeInfo = parkingSpacesTypeInfo.FirstOrDefault(x => x.ParkingSpaceName.Equals("NormalParking"));

            // We check the number of places for normal type of parking
            if ((normalParkingSpaceTypeInfo.CurrentAvalibleNumberSpots - 1) >= 0)
            {
                --normalParkingSpaceTypeInfo.CurrentAvalibleNumberSpots;

                dbContext.Update(normalParkingSpaceTypeInfo);
                dbContext.SaveChanges();

                return true;
            }
            // If there are no free spaces in a normal parking lot, then parking is impossible.
            else
            {
                return false;
            }
        }
    }
}
