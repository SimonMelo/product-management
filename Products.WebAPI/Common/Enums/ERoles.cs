using System.Runtime.Serialization;

namespace Products.WebAPI.Common.Enums;

public enum ERoles
{
    [EnumMember(Value = "Admin")] Admin,
    [EnumMember(Value = "Common")] Common,
}