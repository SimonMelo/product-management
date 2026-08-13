using System.Runtime.Serialization;

namespace Products.WebAPI.Common.Enums;

public enum EMovementType
{
    [EnumMember(Value = "Entrada")] Entrada,
    [EnumMember(Value = "VendaSaida")] VendaSaida,
    [EnumMember(Value = "Ajuste")] Ajuste,
}