using System.Runtime.Serialization;

namespace Products.WebAPI.Common.Enums;

public enum EPaymentMethod
{
    [EnumMember(Value = "Dinheiro")] Dinheiro,
    [EnumMember(Value = "Credito")] Credito,
    [EnumMember(Value = "Debito")] Debito,
    [EnumMember(Value = "Pix")] Pix,
}