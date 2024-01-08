using Carpool.Domain.DTO.Dashboard;

namespace Carpool.Application.Interfaces
{
    public interface IDashboardService
    {
        DashboardStatsDTO GetDashboardStats();
        UserDashboardDTO GetUserDashboard(int userId);
        TripDashboardDTO GetTripDashboard(int tripId);
        DashboardStatsDTO GetGlobalDashboardStats(DateTime startDate, DateTime endDate);
        IEnumerable<string> GetPopularDestinations(int topCount);
        IEnumerable<int> GetMostActiveDrivers(int topCount);
        IEnumerable<int> GetMostBookedTrips(int topCount);
    }
}
