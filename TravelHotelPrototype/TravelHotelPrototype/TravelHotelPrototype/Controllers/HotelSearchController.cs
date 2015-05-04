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

namespace TravelHotelPrototype.Controllers
{

    public class HotelSearchController : Controller
    {
        List<PermittedSuppliersSupplier> Aggregate = new List<PermittedSuppliersSupplier>();
        //
        // GET: /HotelSearch/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Results(SearchCriteria criteria)
        {
            if (criteria.HB)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "HB" }); };
            if (criteria.HW)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "HW" }); };
            if (criteria.LB)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "LB" }); };
            if (criteria.LZ)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "LZ" }); };
            if (criteria.MI)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "MI" }); };
            if (criteria.TO)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "TO" }); };
            if (criteria.U4)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "4U" }); };
            if (criteria.FB)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "FB" }); };
            if (criteria.EA)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "EA" }); };
            if (criteria.AG)
            { Aggregate.Add(new PermittedSuppliersSupplier { Name = "AG" }); };


            if (criteria.provider.Equals("Both")) //Default to no provider specified since specifying a provider removes aggregation of providers-This return TRM and 1G results
            {
                using (var client = new HotelSearchServicePortTypeClient())
                {
                    var request = new HotelSearchAvailabilityReq
                    {
                        TargetBranch = "P7002726",
                        BillingPointOfSaleInfo = new BillingPointOfSaleInfo { OriginApplication = "UAPI" },
                        HotelLocation = new HotelLocation { Location = criteria.cityCode },

                        HotelSearchModifiers = new HotelSearchModifiers
                        {
                            NumberOfAdults = 1,
                            NumberOfRooms = 1,
                            AvailableHotelsOnly = true,
                            PermittedSuppliers = Aggregate.ToArray(),
                            PreferredCurrency = "ZAR",
                        },
                        HotelStay = new HotelStay
                        {
                            CheckinDate = criteria.checkInDate,
                            CheckoutDate = criteria.checkOutDate,
                        },
                    };

                    if (client.ClientCredentials != null)
                    {
                        client.ClientCredentials.UserName.UserName = "Universal API/uAPI1189214086-2752a905";
                        client.ClientCredentials.UserName.Password = "JKD4m3mAxFHkgmC8H3e5P6FfD";
                    }

                    var response = client.service(request);

                    var model = new SearchResultsModel
                    {
                        Criteria = criteria,
                        Results = response
                    };
                    return View("Results", model);
                }
            }
            else
            {
                using (var client = new HotelSearchServicePortTypeClient())
                {
                    var request = new HotelSearchAvailabilityReq
                    {
                        TargetBranch = "P7002726",
                        BillingPointOfSaleInfo = new BillingPointOfSaleInfo { OriginApplication = "UAPI" },
                        HotelLocation = new HotelLocation { Location = criteria.cityCode },

                        HotelSearchModifiers = new HotelSearchModifiers
                        {
                            NumberOfAdults = 1,
                            NumberOfRooms = 1,
                            AvailableHotelsOnly = true,
                            PermittedProviders = new PermittedProviders { Provider = new Provider { Code = criteria.provider } },
                            PermittedSuppliers = Aggregate.ToArray(),
                            PreferredCurrency = "ZAR",
                        },
                        HotelStay = new HotelStay
                        {
                            CheckinDate = criteria.checkInDate,
                            CheckoutDate = criteria.checkOutDate,
                        },
                    };
                    if (client.ClientCredentials != null)
                    {
                        client.ClientCredentials.UserName.UserName = "Universal API/uAPI1189214086-2752a905";
                        client.ClientCredentials.UserName.Password = "JKD4m3mAxFHkgmC8H3e5P6FfD";
                    }

                    var response = client.service(request);

                    var model = new SearchResultsModel
                    {
                        Criteria = criteria,
                        Results = response
                    };
                    return View("Results", model);
                }
            }
        }


        public class ObjectSerializer
        {
            public static T Deserialize<T>(string input) where T : class
            {
                if (input == null)
                    return null;

                var serializer = new XmlSerializer(typeof(T));
                using (var stringReader = new StringReader(input))
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    return serializer.Deserialize(xmlReader) as T;
                }
            }

            public static string Serialize<T>(T input) where T : class
            {
                if (input == null)
                    return null;

                var serializer = new XmlSerializer(typeof(T));
                var sb = new StringBuilder();
                using (var stringWriter = new StringWriter(sb))
                using (var writer = new XmlTextWriter(stringWriter))
                {
                    serializer.Serialize(writer, input);
                    return sb.ToString();
                }
            }
        }
    }
}
