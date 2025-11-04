using ILOS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILOS.Application.Services
{
    public interface IRateService
    {
        Task<IEnumerable<RateConfigurationDto>> GetRateConfigurationsAsync();
        Task<SimulationResponseDto?> CalculateCostAsync(SimulationRequestDto simulationRequest);
    }
}
