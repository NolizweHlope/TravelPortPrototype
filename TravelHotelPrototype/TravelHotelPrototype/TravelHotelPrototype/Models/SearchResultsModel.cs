using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travelport.ServiceProxy.KestrelData;

namespace TravelHotelPrototype.Models
{
    public class SearchResultsModel
    {
        public SearchCriteria Criteria { get; set; }

        public BaseHotelSearchRsp Results { get; set; }
    }
}