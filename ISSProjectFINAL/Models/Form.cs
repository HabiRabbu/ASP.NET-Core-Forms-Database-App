using ISSProjectFINAL.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISSProjectFINAL.Models
{
    public record Form
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Category Category { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Date { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly Time { get; set; }
        public string Details { get; set; } = string.Empty;
        public bool InformationVerified { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DateCreated { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly TimeCreated { get; set; }
    };
}