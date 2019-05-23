using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class X
    {
        string location { get; set; }
        Location locations { get; set; }

        public class Location
        {
            City city = new City();
            List<Salon> salons = new List<Salon>();
        }

        public class City
        {
            int id { get; set; }
            string name { get; set; }
        }

        public class Salon
        {
            int id { get; set; }
            string name { get; set; }
            string address { get; set; }
        }

    }
    
}
