using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using GoogleApi.Entities.Maps.Geocoding;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace IT391M5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Coordinate currentcord;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
           string url = urlGenerated();

            currentcord = GetCoordinates(url);

        }

        public string urlGenerated()
        {
            string streetNum = Request.Form["StreetNum"];
            string streetName = Request.Form["StreetName"];
            string city = Request.Form["City"];
            string state = Request.Form["State"];
            string zip = Request.Form["Zip"];
            string country = Request.Form["Country"];

            string url = ($"https://maps.googleapis.com/maps/api/geocode/json?address={streetNum}%20{streetName}%20{city}%20{state}%20{zip}%20{country}&key=AIzaSyAmbzpCKh8PdP-8nkGgYJQxlus2SeCjpZY");

            return url;
        }

        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
                string jsonData = client.DownloadString(region);

                JObject parsedJson = JObject.Parse(jsonData);
                double lat = (double) parsedJson["results"][0]["geometry"]["location"]["lat"];
                double lng = (double) parsedJson["results"][0]["geometry"]["location"]["lng"];

                return new Coordinate(lat, lng);
            }
        }

        public struct Coordinate
        {
            private double lat;
            private double lng;

            public Coordinate(double latitude, double longitude)
            {
                lat = latitude;
                lng = longitude;
            }

            public double Latitude { get { return lat; } set { lat = value; } }
            public double Longitude { get { return lng; } set { lng = value; } }

        }
    }
}
