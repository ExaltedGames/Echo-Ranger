using System.Text.Json.Serialization;

namespace HackmonInternals.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HackmonFamily
{
   Construct,
   Aquatic,
   Beast,
   Avian,
   Reptile,
   Arthropod
}