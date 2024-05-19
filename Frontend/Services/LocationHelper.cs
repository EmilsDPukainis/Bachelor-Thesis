using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class LocationHelper
    {
        private const double EarthRadiusKm = 6371;

        public bool IsWithinRadius(double userLatitude, double userLongitude, double workLatitude, double workLongitude, double radiusKm)
        {
            
            var userLatitudeRad = DegreesToRadians(userLatitude);
            var userLongitudeRad = DegreesToRadians(userLongitude);
            var workLatitudeRad = DegreesToRadians(workLatitude);
            var workLongitudeRad = DegreesToRadians(workLongitude);

            var deltaLatitude = userLatitudeRad - workLatitudeRad;
            var deltaLongitude = userLongitudeRad - workLongitudeRad;

            var a = Math.Pow(Math.Sin(deltaLatitude / 2), 2) +
                    Math.Cos(workLatitudeRad) * Math.Cos(userLatitudeRad) *
                    Math.Pow(Math.Sin(deltaLongitude / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;

            return distance <= radiusKm;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}