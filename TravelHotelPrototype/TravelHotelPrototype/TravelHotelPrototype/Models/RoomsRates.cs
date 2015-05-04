using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Travelport.ServiceProxy.KestrelData;
using TravelHotelPrototype.Models;


namespace TravelHotelPrototype.Models
{
    public class RoomsRates
    {
        public BaseHotelDetailsRsp Rates { get; set; }
    }
}