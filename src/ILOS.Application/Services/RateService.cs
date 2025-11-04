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

        public async Task<SimulationResponseDto?> CalculateCostAsync(SimulationRequestDto simulationRequest)
        {

            var response = await _supabase
                .From<RateConfiguration>()
                .Where(model => model.RateName == simulationRequest.RateName)
                .Get();


            var rateConfig = response.Models.FirstOrDefault();


            if (rateConfig == null)
            {


                return null;
            }


            decimal weightCost = simulationRequest.Weight * rateConfig.RatePerPound;
            decimal distanceCost = simulationRequest.Distance * rateConfig.RatePerMile;

            decimal totalCost = rateConfig.BaseRate + weightCost + distanceCost;

            var responseDto = new SimulationResponseDto
            {
                RateName = rateConfig.RateName,
                BaseRate = rateConfig.BaseRate,
                WeightCost = weightCost,
                DistanceCost = distanceCost,
                TotalCost = totalCost
            };


            return responseDto;
        }


    }
}
