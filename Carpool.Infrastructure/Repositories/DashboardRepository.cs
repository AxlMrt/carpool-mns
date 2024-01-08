using Carpool.Domain.Common;
using Carpool.Domain.Entities;
using Carpool.Infrastructure.Context;

public class DashboardRepository
{
    private readonly CarpoolDbContext _context;

    public DashboardRepository(CarpoolDbContext context)
    {
        _context = context;
    }

    // Statistiques Globales
    public int GetTotalUsersCount()
    {
        return _context.Users.Count();
    }

    public int GetTotalTripsCount()
    {
        return _context.Trips.Count();
    }

    public int GetTotalReservationsCount()
    {
        return _context.Reservations.Count();
    }

    // Gestion des Utilisateurs
    public User GetUserById(int userId)
    {
        return _context.Users.FirstOrDefault(u => u.Id == userId);
    }

    public IEnumerable<Car> GetUserCars(int userId)
    {
        return _context.Cars.Where(c => c.OwnerId == userId).ToList();
    }

    public IEnumerable<Address> GetUserAddresses(int userId)
    {
        return _context.Addresses.Where(a => a.UserId == userId).ToList();
    }

    public IEnumerable<Reservation> GetUserReservations(int userId)
    {
        return _context.Reservations.Where(r => r.UserId == userId).ToList();
    }

    // Gestion des Trajets
    public IEnumerable<Trip> GetDriverTrips(int driverId)
    {
        return _context.Trips.Where(t => t.DriverId == driverId).ToList();
    }

    public IEnumerable<Reservation> GetTripReservations(int tripId)
    {
        return _context.Reservations.Where(r => r.TripId == tripId).ToList();
    }

    // Feedback et Évaluations
    public IEnumerable<Feedback> GetUserFeedback(int userId)
    {
        return _context.Feedbacks.Where(f => f.UserId == userId).ToList();
    }

    public IEnumerable<Feedback> GetTripFeedback(int tripId)
    {
        return _context.Feedbacks.Where(f => f.TripId == tripId).ToList();
    }

    // Communication et Messages
    public IEnumerable<Message> GetTripMessages(int tripId)
    {
        return _context.Messages.Where(m => m.TripId == tripId).ToList();
    }

    public IEnumerable<Message> GetUserMessages(int userId)
    {
        return _context.Messages.Where(m => m.RecipientId == userId).ToList();
    }

    // Méthodes Utilitaires
    public IEnumerable<Trip> SearchTripsByStreet(string searchTerm)
    {
        // Logique de recherche basée sur le terme spécifique
        return _context.Trips.Where(t => t.DestinationAddress.Street.Contains(searchTerm)).ToList();
    }

    public bool ConfirmReservation(int reservationId)
    {
        Reservation reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation != null)
        {
            reservation.Status = ReservationStatus.Confirmed;
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool CancelReservation(int reservationId)
    {
        Reservation reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return true;
        }

        return false; 
    }


    // Données Agrégées
    public int GetActiveTripsCount(DateTime startDate, DateTime endDate)
    {
        return _context.Trips.Count(t => t.DepartureTime >= startDate && t.DepartureTime <= endDate);
    }

    public int GetDriversUsersCount()
    {
        return _context.Trips.Select(t => t.DriverId).Distinct().Count();
    }

    public IEnumerable<string> GetPopularDestinations(int topCount)
    {
        IEnumerable<string> popularDestinations = _context.Trips
            .GroupBy(t => t.DestinationAddress.City)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key)
            .ToList();

        return popularDestinations;
    }

    // Statistiques Basées sur l'Utilisation de l'Application
    public double GetAverageTripsPerDay(DateTime startDate, DateTime endDate)
    {
        double totalDays = (endDate - startDate).TotalDays;
        int totalTrips = _context.Trips.Count(t => t.DepartureTime >= startDate && t.DepartureTime <= endDate);
        return totalTrips / totalDays;
    }

    public IEnumerable<int> GetMostActiveDrivers(int topCount)
    {
        IEnumerable<int> mostActiveDrivers = _context.Trips
            .GroupBy(t => t.DriverId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.Key)
            .ToList();

        return mostActiveDrivers;
    }

    public IEnumerable<Trip> GetMostBookedTrips(int topCount)
    {
        IEnumerable<Trip> mostBookedTrips = _context.Trips
            .OrderByDescending(t => t.Reservations.Count)
            .Take(topCount)
            .ToList();

        return mostBookedTrips;
    }
}
