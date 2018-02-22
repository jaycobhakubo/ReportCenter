using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTI.Modules.ReportCenter.Data
{
    public class ProductTypes
    {
        public int ProdTypeID { get; set; }
        public string ProdTypeName { get; set; }

        public ProductTypes(int id, string name)
        {
            ProdTypeID = id;
            ProdTypeName = name;
        }

    }
}
