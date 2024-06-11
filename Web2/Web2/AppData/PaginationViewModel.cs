using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.AppData
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; }
        public int NumberRecords { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}