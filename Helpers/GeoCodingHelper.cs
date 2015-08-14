using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

namespace MealsToGo.Helpers
{
    public class GeoCodingHelper : Controller
    {
        public static GLatLong GetLatLong(string query)
        {
            WebRequest request = WebRequest
               .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="+ HttpUtility.UrlEncode(query));

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement != null && latitudeElement != null)
                    {
                        return new GLatLong
                        {
                            Longitude = Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                            Latitude = Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture)
                        };
                    }
                }
            }

            return null;
        }


        public static string GetAddress(GLatLong latlong)
        {
            WebRequest request = WebRequest
               .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address=" + latlong);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    string address = document.Descendants("formatted_address").FirstOrDefault().ToString(); 
                   return address;
                }
            }

            
        }
    }
}
