using System.Text.RegularExpressions;
using Carpool.Application.DTO.Car;
using Carpool.Domain.Entities;

namespace Carpool.Application.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }

        public static bool IsStrongPassword(string password)
        {
            // Minimal length 8char
            // At least 1 upper and lower case letter
            // At least one special char
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{8,}$");
        }

        public static bool IsValidAddress(Address addressDTO)
        {
            return !string.IsNullOrEmpty(addressDTO.Street)
                && !string.IsNullOrEmpty(addressDTO.City)
                && !string.IsNullOrEmpty(addressDTO.PostalCode)
                && !string.IsNullOrEmpty(addressDTO.Country)
                && IsValidPostalCode(addressDTO.PostalCode)
                && IsValidLatitude(addressDTO.Latitude)
                && IsValidLongitude(addressDTO.Longitude);
        }

        private static bool IsValidPostalCode(string postalCode)
        {
            return Regex.IsMatch(postalCode, @"^\d{5}$");
        }

        private static bool IsValidLatitude(double lat)
        {
            return lat >= -90 && lat <= 90;
        }

        private static bool IsValidLongitude(double lon)
        {
            return lon >= -180 && lon <= 180;
        }

        public static bool IsCarValid(Car car)
        {
            return !string.IsNullOrEmpty(car.Brand)
                && !string.IsNullOrEmpty(car.Model)
                && car.Year > 1900 && car.Year <= DateTime.Now.Year
                && !string.IsNullOrEmpty(car.Color)
                && car.NumberOfSeats > 0
                && !string.IsNullOrEmpty(car.LicensePlate)
                && car.TechnicalInspectionDate > DateTime.Now
                && car.InsuranceExpirationDate > DateTime.Now;
        }
    }
}