using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace YSharp.Common;

[ExcludeFromCodeCoverage]
public static class StaticConfig
{
    public static readonly CultureInfo numberCulture = new CultureInfo("en-US");
}
