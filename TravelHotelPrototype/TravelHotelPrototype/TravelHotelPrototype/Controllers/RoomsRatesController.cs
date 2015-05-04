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
    public class RoomsRatesController : Controller
    {
        //
        // GET: /RoomsRates/

        public ActionResult RoomsRates(string hostToken, DateTime checkInDate, DateTime checkOutDate, string TraceId, string Name, string HotelChain, string HotelCode)
        {
            using (var client = new HotelDetailsServicePortTypeClient())
            {
                var request = new HotelDetailsReq
                {
                    TargetBranch = "P7002726",
                    BillingPointOfSaleInfo = new BillingPointOfSaleInfo { OriginApplication = "UAPI" },
                    ReturnMediaLinks = true,
                    TraceId = TraceId,
                    AuthorizedBy = "Travelport",

                    HotelProperty = new HotelProperty
                    {
                        Name = Name,
                        HotelChain = HotelChain,
                        HotelCode = HotelCode,
                    },
                    HotelDetailsModifiers = new HotelDetailsModifiers
                    {
                        PermittedProviders = new PermittedProviders { Provider = new Provider { Code = "TRM" } },
                        NumberOfAdults = 1,
                        RateRuleDetail = (typeRateRuleDetail)1, //Complete Details
                        RateSupplier = "TP_AG_EN",
                        PreferredCurrency = "ZAR",
                        HotelStay = new HotelStay
                        {
                            CheckinDate = checkInDate,
                            CheckoutDate = checkOutDate,
                        }
                    },
                    HostToken = new HostToken { Host = "TRM", Value = hostToken },
                };

                if (client.ClientCredentials != null)
                {
                    client.ClientCredentials.UserName.UserName = "Universal API/uAPI1189214086-2752a905";
                    client.ClientCredentials.UserName.Password = "JKD4m3mAxFHkgmC8H3e5P6FfD";
                }

                var response = client.service(request);

                var model = new RoomsRates
                {
                    Rates = response
                };
                return View("RoomsRates", model);
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
