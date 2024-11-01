using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Local_Canteen_Optimizer.Model
{
    class ApiUser
    {
        public string Gender { get; set; }

        public Name Name { get; set; }

        public Location Location { get; set; }
    }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Location
    {
        public Street Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Postcode { get; set; }
    }

    public class Street
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
