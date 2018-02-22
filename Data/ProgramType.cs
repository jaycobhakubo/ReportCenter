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
    public class ProgramType
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }

        public ProgramType(int id, string name)
        {
            ProgramId = id;
            ProgramName = name;
        }
    }
}
