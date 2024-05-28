using System.Text.Json.Serialization;

namespace HackmonInternals.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HackmonType
{
    Basic,
    Fire,
    Water,
    Plant,
    Earth,
    Wind,
    Jolt,
    Spirit,
    Mech,
    Chem,
    Frost
}