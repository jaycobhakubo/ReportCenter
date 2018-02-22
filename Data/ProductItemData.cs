using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTI.Modules.ReportCenter
{
    class ProductItemData
    {
        public int ProductItemID { get; set; }
        public string ProductItemName { get; set; }

        public ProductItemData(int id, string name)
        {
            ProductItemID = id;
            ProductItemName = name;
        }

    }
}
