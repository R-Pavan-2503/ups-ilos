using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace ILOS.Application.Models
{
    [Table("RateConfigurations")]
    public class RateConfiguration : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("rate_name")]
        public string RateName { get; set; } = string.Empty;

        [Column("base_rate")]
        public decimal BaseRate { get; set; }

        [Column("rate_per_mile")]
        public decimal RatePerMile { get; set; }

        [Column("rate_per_pound")]
        public decimal RatePerPound { get; set; }
    }
}
