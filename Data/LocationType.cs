using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTI.Modules.ReportCenter.Data
{
    /// <summary>
    /// The class that defines the location types
    /// </summary>
    public enum LocationTypes
    {
        /// <summary>
        /// All Locations (0)
        /// </summary>
        All = 0,
        /// <summary>
        /// Physical locations (1)
        /// </summary>
        Physical = 1,
        /// <summary>
        /// Machine locations (2)
        /// </summary>
        Machine = 2,
        /// <summary>
        /// Staff locations (3)
        /// </summary>
        Staff = 3
    }

    public class LocationType
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }

        public LocationType(int id, string name)
        {
            LocationID = id;
            LocationName = name;
        }

    }
}
