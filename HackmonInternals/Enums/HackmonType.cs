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
	Electric,
	Spirit,
	Metal,
	Chem,
	Frost,
	Astral
}