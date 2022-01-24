using ISSProjectFINAL.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISSProjectFINAL.Models
{
    public record Category
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
    };
}
