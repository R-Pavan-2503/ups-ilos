using ILOS.Application.DTOs;
using ILOS.Application.Models;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILOS.Application.Services
{
    public class RateService : IRateService
    {
        private readonly Client _supabase;

        public RateService(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<RateConfigurationDto>> GetRateConfigurationsAsync()
        {
            var response = await _supabase
                .From<RateConfiguration>()
                .Get();

            var dtos = response.Models.Select(model => new RateConfigurationDto
            {
                Id = model.Id,
                RateName = model.RateName,
                BaseRate = model.BaseRate,
                RatePerMile = model.RatePerMile,
                RatePerPound = model.RatePerPound
            });

            return dtos;
        }

        public async Task<decimal> CalculateCostAsync(SimulationRequestDto simulationRequest)
        {
            // 1. Fetch the specific rate config from Supabase
            var response = await _supabase
                .From<RateConfiguration>()
                .Where(model => model.RateName == simulationRequest.RateName) 
                .Get();

            // 2. Get the *first* matching result
            var rateConfig = response.Models.FirstOrDefault();

            // 3. Check if we found it. If not, throw an error.
            if (rateConfig == null)
            {
                // We'll just return -1 for now to signal an error
                // In a real app, we'd throw an exception
                return -1;
            }

            // 4. Perform the calculation based on your proposal's logic
            decimal weightCost = simulationRequest.Weight * rateConfig.RatePerPound;
            decimal distanceCost = simulationRequest.Distance * rateConfig.RatePerMile;

            decimal totalCost = rateConfig.BaseRate + weightCost + distanceCost;

            return totalCost;
        }


    }
}
