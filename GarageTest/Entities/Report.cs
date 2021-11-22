using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageTest.Entities
{
    /// <summary>
    ///     The entity of reporting
    /// </summary>
    public class Report
    {
        public int AvailableParkingSpaces { get; set; }
        public int PercentageOfParkingSpacesUsed { get; set; }
        public Dictionary<string, int> PercentageOfParkingSpacesUsedByParkingType { get; set; }
        public double OutstandingProfit { get; set; }
        public double HistoricalOverallProfit { get; set; }

        public Report()
        {
            AvailableParkingSpaces = 0;
            OutstandingProfit = 0;
            HistoricalOverallProfit = 0;
            PercentageOfParkingSpacesUsedByParkingType = new Dictionary<string, int>();
        }
    }
}
