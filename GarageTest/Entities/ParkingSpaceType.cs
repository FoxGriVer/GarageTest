using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageTest.Entities
{
    /// <summary>
    ///     The entity of the parking type
    /// </summary>
    public class ParkingSpaceType
    {
        public int Id { get; set; }
        public string ParkingSpaceName { get; set; }
        public int MaximumNumberSpots { get; set; }
        public int CurrentAvalibleNumberSpots { get; set; }
        public double PricePerHour { get; set; }

        public int IdParkingGarage { get; set; }
    }
}
