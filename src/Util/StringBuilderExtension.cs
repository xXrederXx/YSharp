using System.Text;

namespace YSharp.Util;
public static class StringBuilderExtension {
    public static string GetAndClear(this StringBuilder self)
    {
        string value = self.ToString();
        self.Clear();
        return value;
    }
}
