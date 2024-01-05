using System.Text.RegularExpressions;
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

        public static string IsValidAddress(Address addressDTO)
        {
            if (string.IsNullOrEmpty(addressDTO.Street))
                return "Street cannot be empty.";

            if (string.IsNullOrEmpty(addressDTO.City))
                return "City cannot be empty.";

            if (string.IsNullOrEmpty(addressDTO.PostalCode))
                return "Postal code cannot be empty.";

            if (string.IsNullOrEmpty(addressDTO.Country))
                return "Country cannot be empty.";

            if (!IsValidPostalCode(addressDTO.PostalCode))
                return "Invalid postal code format.";

            if (!IsValidLatitude(addressDTO.Latitude))
                return "Invalid latitude value.";

            if (!IsValidLongitude(addressDTO.Longitude))
                return "Invalid longitude value.";

            return "Valid";
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

        public static string IsCarValid(Car car)
        {
            if (string.IsNullOrEmpty(car.Brand))
                return "Brand is required.";

            if (string.IsNullOrEmpty(car.Model))
                return "Model is required.";

            if (car.Year <= 1900 || car.Year > DateTime.Now.Year)
                return "Invalid year.";

            if (string.IsNullOrEmpty(car.Color))
                return "Color is required.";

            if (car.NumberOfSeats <= 0)
                return "Number of seats should be greater than zero.";

            if (string.IsNullOrEmpty(car.LicensePlate))
                return "License plate is required.";

            if (car.TechnicalInspectionDate <= DateTime.Now)
                return "Technical inspection date should be in the future.";

            if (car.InsuranceExpirationDate <= DateTime.Now)
                return "Insurance expiration date should be in the future.";

            return "Valid";
        }
        
        public static string IsValidFeedback(Feedback feedback)
        {
            if (string.IsNullOrEmpty(feedback.Comment))
                return "Feedback comment cannot be empty.";

            if (feedback.Rating < 1 || feedback.Rating > 5)
                return "Feedback rating should be between 1 and 5.";

            if (feedback.UserId <= 0)
                return "Invalid user ID in feedback.";

            return "Valid";
        }

        public static string IsValidTrip(Trip trip)
        {
            if (trip.CarId <= 0)
                return "Invalid car ID in trip.";

            if (trip.DepartureAddressId == null || trip.DepartureAddressId <= 0)
                return "Invalid departure address ID in trip.";

            if (trip.DestinationAddressId == null || trip.DestinationAddressId <= 0)
                return "Invalid destination address ID in trip.";

            if (trip.DepartureTime <= DateTime.Now)
                return "Departure time should be in the future.";

            if (trip.AvailableSeats <= 0)
                return "Available seats should be greater than zero.";

            return "Valid";
        }
    }
}