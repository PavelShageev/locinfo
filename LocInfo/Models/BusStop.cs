using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocInfo.Models
{
    public class BusStop
    {
        public double Distance { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
    }
}