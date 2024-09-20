using System.ComponentModel;

namespace CraftersCloud.Core.Enum;

public class CodeAndDescriptionAttribute : DescriptionAttribute
{
    public CodeAndDescriptionAttribute(string code, string description) : base(description) => Code = code;

    public string Code { get; }
}