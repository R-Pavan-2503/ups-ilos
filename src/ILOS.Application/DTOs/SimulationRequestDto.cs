namespace ILOS.Application.DTOs
{
    // This class holds the incoming data for a simulation request
    public class SimulationRequestDto
    {
        // We use decimal for financial/measurement accuracy
        public decimal Weight { get; set; }
        public decimal Distance { get; set; }
        public string RateName { get; set; } = string.Empty;
    }
}