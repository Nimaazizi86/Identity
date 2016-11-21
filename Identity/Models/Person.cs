using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PersonalNumber { get; set; }
        //public bool Sex;
        public int? countryId { get; set; }
        public Country country { get; set; }
        public int? cityId { get; set; }
        public City city { get; set; }
        public string creater { get; set; }
    }

}