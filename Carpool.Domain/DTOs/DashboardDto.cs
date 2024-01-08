using Carpool.Application.DTO.Car;
using Carpool.Domain.DTO.Address;
using Carpool.Domain.DTO.Reservation;
using Carpool.Domain.DTOs;
using Carpool.Domain.DTOs.Feedback;

namespace Carpool.Domain.DTO.Dashboard
{
    public class DashboardStatsDTO
    {
        public int TotalUsersCount { get; set; }
        public int TotalTripsCount { get; set; }
        public int TotalReservationsCount { get; set; }
        public int ActiveTripsCount { get; set; }
        public int DriversUsersCount { get; set; }
        public int UniqueUsersCount { get; set; }
    }

    public class UserDashboardDTO
    {
        public int UserId { get; set; }
        public List<CarDTO> Cars { get; set; }
        public List<AddressDTO> Addresses { get; set; }
        public List<ReservationDTO> Reservations { get; set; }
    }

    public class TripDashboardDTO
    {
        public int TripId { get; set; }
        public List<ReservationDTO> Reservations { get; set; }
        public List<FeedbackDTO> Feedback { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }

    public class GlobalStatsDTO
    {
        public int ActiveTripsCount { get; set; }
        public int DriversUsersCount { get; set; }
        public int UniqueUsersCount { get; set; }
    }
}