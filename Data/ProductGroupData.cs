using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTI.Modules.ReportCenter
{
    class ProductGroupData
    {
        public bool IsActive { get; set; }
        public int ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public ProductGroupData(bool isActive, int id, string name)
        {
            IsActive = isActive;
            ProductGroupID = id;
            ProductGroupName = name;
        }

    }
}
