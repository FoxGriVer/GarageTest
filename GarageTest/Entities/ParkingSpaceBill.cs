using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GarageTest.Entities
{
    /// <summary>
    ///     The entity of the parking receipt
    /// </summary>
    public class ParkingSpaceBill
    {
        public int Id { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public double TotalAmount { get; set; }
        public TimeSpan TimeSpentInTheParking { get; set; }
        public bool Paid { get; set; }

        /// <summary>
        ///     We use this field to check whether the license plate of the car exists among the registered ones or not. 
        ///     In practice, we check this is essentially a Car, but since in this case we do not need a car, we can check this way.
        /// </summary>
        [NotMapped]
        public bool CarLicensePlateRegistered { get; set; }

        public int ParkingSpaceTypeId { get; set; }
        public ParkingSpaceType ParkingSpaceType { get; set; }
    }
}
