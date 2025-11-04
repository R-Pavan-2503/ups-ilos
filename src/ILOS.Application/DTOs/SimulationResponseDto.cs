namespace ILOS.Application.DTOs
{

    public class SimulationResponseDto
    {
        public string RateName { get; set; } = string.Empty;
        public decimal BaseRate { get; set; }
        public decimal WeightCost { get; set; }
        public decimal DistanceCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}