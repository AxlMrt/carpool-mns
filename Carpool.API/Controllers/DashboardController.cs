using Carpool.Application.Exceptions;
using Carpool.Application.Interfaces;
using Carpool.Domain.DTO.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Carpool.API.Controllers
{
    [Authorize]
    public class DashboardController : BaseApiController, IExceptionFilter
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public ActionResult<DashboardStatsDTO> GetDashboardStats()
        {
            var stats = _dashboardService.GetDashboardStats();
            return Ok(stats);
        }

        [HttpGet("user/{userId}")]
        public ActionResult<UserDashboardDTO> GetUserDashboard(int userId)
        {
            var userDashboard = _dashboardService.GetUserDashboard(userId);
            return Ok(userDashboard);
        }

        [HttpGet("trip/{tripId}")]
        public ActionResult<TripDashboardDTO> GetTripDashboard(int tripId)
        {
            var tripDashboard = _dashboardService.GetTripDashboard(tripId);
            return Ok(tripDashboard);
        }

        [HttpGet("global")]
        public ActionResult<DashboardStatsDTO> GetGlobalDashboardStats(DateTime startDate, DateTime endDate)
        {
            var globalStats = _dashboardService.GetGlobalDashboardStats(startDate, endDate);
            return Ok(globalStats);
        }

        [HttpGet("popular-destinations")]
        public ActionResult<IEnumerable<string>> GetPopularDestinations(int topCount)
        {
            var popularDestinations = _dashboardService.GetPopularDestinations(topCount);
            return Ok(popularDestinations);
        }

        [HttpGet("active-drivers")]
        public ActionResult<IEnumerable<int>> GetMostActiveDrivers(int topCount)
        {
            var activeDrivers = _dashboardService.GetMostActiveDrivers(topCount);
            return Ok(activeDrivers);
        }

        [HttpGet("booked-trips")]
        public ActionResult<IEnumerable<int>> GetMostBookedTrips(int topCount)
        {
            var bookedTrips = _dashboardService.GetMostBookedTrips(topCount);
            return Ok(bookedTrips);
        }

        [NonAction]
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            int statusCode = 500;

            if (exception is NotFoundException)
            {
                statusCode = 404;
            }
            else if (exception is BadRequestException || exception is ArgumentException)
            {
                statusCode = 400;
            }

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }
}
