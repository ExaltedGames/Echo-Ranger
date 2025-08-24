using System.Text.Json.Serialization;

namespace HackmonInternals.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttackType { None, Special, Physical }