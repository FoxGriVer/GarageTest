using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GarageTest.Entities.DbContext
{
    /// <summary>
    ///     Garage Database Context
    /// </summary>
    public class GarageDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ParkingSpaceType> ParkingSpaceTypes { get; set; }
        public DbSet<ParkingSpaceBill> ParkingSpaceBills { get; set; }
        public DbSet<ParkingGarage> ParkingGarages { get; set; }

        public GarageDbContext(DbContextOptions<GarageDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
