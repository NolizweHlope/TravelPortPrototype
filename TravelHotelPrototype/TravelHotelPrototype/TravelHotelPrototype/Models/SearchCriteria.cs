using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Travelport.ServiceProxy.KestrelData;

namespace TravelHotelPrototype.Models
{

    public class SearchCriteria
    {
        [DisplayName("City Code")]
        public string cityCode { get; set; }
        [DisplayName("Check In Date")]
        public DateTime checkInDate { get; set; }
        [DisplayName("Check Out Date")]
        public DateTime checkOutDate { get; set; }
        [DisplayName("Provider")]
        public string provider { get; set; }
        public bool AG { get; set; }
        public bool EA { get; set; }
        public bool FB { get; set; }
        public bool HB { get; set; }
        public bool HW { get; set; }
        public bool LZ { get; set; }
        public bool LB { get; set; }
        public bool U4 { get; set; }
        public bool MI { get; set; }
        public bool TO { get; set; }
    }
}