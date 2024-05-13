using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class LocationHelper
    { // Radius of the Earth in kilometers
        private const double EarthRadiusKm = 6371;

        public bool IsWithinRadius(double userLatitude, double userLongitude, double workLatitude, double workLongitude, double radiusKm)
        {
            // Convert latitude and longitude from degrees to radians
            var userLatitudeRad = DegreesToRadians(userLatitude);
            var userLongitudeRad = DegreesToRadians(userLongitude);
            var workLatitudeRad = DegreesToRadians(workLatitude);
            var workLongitudeRad = DegreesToRadians(workLongitude);

            // Calculate the differences in latitude and longitude
            var deltaLatitude = userLatitudeRad - workLatitudeRad;
            var deltaLongitude = userLongitudeRad - workLongitudeRad;

            // Calculate the distance using the Haversine formula
            var a = Math.Pow(Math.Sin(deltaLatitude / 2), 2) +
                    Math.Cos(workLatitudeRad) * Math.Cos(userLatitudeRad) *
                    Math.Pow(Math.Sin(deltaLongitude / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;

            // Check if the distance is within the specified radius
            return distance <= radiusKm;
        }

        // Helper method to convert degrees to radians
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}