#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2013 FortuNet
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTI.Modules.ReportCenter.Data
{
    public class PosMenuType
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }

        public PosMenuType(int id, string name)
        {
            MenuId = id;
            MenuName = name;
        }
    }
}
