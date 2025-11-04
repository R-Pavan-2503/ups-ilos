namespace ILOS.Application.DTOs
{
    // This is a simple, clean "Data Transfer Object"
    public class RateConfigurationDto
    {
        public Guid Id { get; set; }
        public string RateName { get; set; } = string.Empty;
        public decimal BaseRate { get; set; }
        public decimal RatePerMile { get; set; }
        public decimal RatePerPound { get; set; }
    }
}