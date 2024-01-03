namespace Carpool.Domain.DTOs
{
    public class CarCreateDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int NumberOfSeats { get; set; }
        public string LicensePlate { get; set; }
        public DateTime TechnicalInspectionDate { get; set; }
        public DateTime InsuranceExpirationDate { get; set; }

        public int OwnerId { get; set; }
    }
}