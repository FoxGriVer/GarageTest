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
    ///     The controller is responsible for working with the types of parking spaces
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingTypeController : Controller
    {
        GarageDbContext dbContext;

        public ParkingTypeController(GarageDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        ///     Get information on all types of parking
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetParkingSpaces")]
        public async Task<ActionResult<IEnumerable<ParkingSpaceType>>> GetParkingSpaceTypes()
        {
            return await dbContext.ParkingSpaceTypes.ToListAsync();
        }

        /// <summary>
        ///     Get information on all the specific type of parking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetParkingSpaceTypesById/{id}")]
        public async Task<ActionResult<ParkingSpaceType>> GetParkingSpaceTypesById(int id)
        {
            ParkingSpaceType parkingSpaceType = await dbContext.ParkingSpaceTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (parkingSpaceType == null)
            {
                return NotFound();
            }

            return new ObjectResult(parkingSpaceType);
        }
    }
}
