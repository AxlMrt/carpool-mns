using Carpool.Domain.Entities;

namespace Carpool.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        int GetTotalUsersCount();
        int GetTotalTripsCount();
        int GetTotalReservationsCount();

        User GetUserById(int userId);
        IEnumerable<Car> GetUserCars(int userId);
        IEnumerable<Address> GetUserAddresses(int userId);
        IEnumerable<Reservation> GetUserReservations(int userId);

        IEnumerable<Trip> GetDriverTrips(int driverId);
        IEnumerable<Reservation> GetTripReservations(int tripId);

        IEnumerable<Feedback> GetUserFeedback(int userId);
        IEnumerable<Feedback> GetTripFeedback(int tripId);

        IEnumerable<Message> GetTripMessages(int tripId);
        IEnumerable<Message> GetUserMessages(int userId);

        IEnumerable<Trip> SearchTripsByStreet(string searchTerm);
        bool ConfirmReservation(int reservationId);
        bool CancelReservation(int reservationId);

        int GetActiveTripsCount(DateTime startDate, DateTime endDate);
        int GetDriversUsersCount();
        IEnumerable<string> GetPopularDestinations(int topCount);

        double GetAverageTripsPerDay(DateTime startDate, DateTime endDate);
        IEnumerable<int> GetMostActiveDrivers(int topCount);
        IEnumerable<Trip> GetMostBookedTrips(int topCount);
    }
}
