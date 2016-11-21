using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity.Models
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country country { get; set; }
    }
}