using Carpool.Application.DTO.Car;
using Carpool.Application.Interfaces;
using Carpool.Application.Utils;
using Carpool.Domain.DTO.Address;
using Carpool.Domain.DTO.Dashboard;
using Carpool.Domain.DTO.Reservation;
using Carpool.Domain.DTOs;
using Carpool.Domain.DTOs.Feedback;
using Carpool.Domain.Interfaces;

namespace Carpool.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public DashboardStatsDTO GetDashboardStats()
        {
            int totalUsersCount = _dashboardRepository.GetTotalUsersCount();
            int totalTripsCount = _dashboardRepository.GetTotalTripsCount();
            int totalReservationsCount = _dashboardRepository.GetTotalReservationsCount();

            return new DashboardStatsDTO
            {
                TotalUsersCount = totalUsersCount,
                TotalTripsCount = totalTripsCount,
                TotalReservationsCount = totalReservationsCount
            };
        }

        public UserDashboardDTO GetUserDashboard(int userId)
        {
            var userCars = _dashboardRepository.GetUserCars(userId);
            var userAddresses = _dashboardRepository.GetUserAddresses(userId);
            var userReservations = _dashboardRepository.GetUserReservations(userId);

            return new UserDashboardDTO
            {
                UserId = userId,
                Cars = userCars.Select(u => ObjectUpdater.MapObject<CarDTO>(u)).ToList(),
                Addresses = userAddresses.Select(u => ObjectUpdater.MapObject<AddressDTO>(u)).ToList(),
                Reservations = userReservations.Select(u => ObjectUpdater.MapObject<ReservationDTO>(u)).ToList()
            };
        }

        public TripDashboardDTO GetTripDashboard(int tripId)
        {
            var tripReservations = _dashboardRepository.GetTripReservations(tripId);
            var tripFeedback = _dashboardRepository.GetTripFeedback(tripId);
            var tripMessages = _dashboardRepository.GetTripMessages(tripId);

            return new TripDashboardDTO
            {
                TripId = tripId,
                Reservations = tripReservations.Select(u => ObjectUpdater.MapObject<ReservationDTO>(u)).ToList(),
                Feedback = tripFeedback.Select(u => ObjectUpdater.MapObject<FeedbackDTO>(u)).ToList(),
                Messages = tripMessages.Select(u => ObjectUpdater.MapObject<MessageDTO>(u)).ToList()
            };
        }

        public DashboardStatsDTO GetGlobalDashboardStats(DateTime startDate, DateTime endDate)
        {
            int activeTripsCount = _dashboardRepository.GetActiveTripsCount(startDate, endDate);
            int driversUsersCount = _dashboardRepository.GetDriversUsersCount();
            int uniqueUsersCount = _dashboardRepository.GetDriversUsersCount();

            return new DashboardStatsDTO
            {
                ActiveTripsCount = activeTripsCount,
                DriversUsersCount = driversUsersCount,
                UniqueUsersCount = uniqueUsersCount
            };
        }

        public IEnumerable<string> GetPopularDestinations(int topCount)
        {
            return _dashboardRepository.GetPopularDestinations(topCount);
        }

        public IEnumerable<int> GetMostActiveDrivers(int topCount)
        {
            return _dashboardRepository.GetMostActiveDrivers(topCount);
        }

        public IEnumerable<int> GetMostBookedTrips(int topCount)
        {
            return _dashboardRepository.GetMostBookedTrips(topCount).Select(trip => trip.Id);
        }
    }
}
