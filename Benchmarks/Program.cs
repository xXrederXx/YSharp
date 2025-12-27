using YSharp.Benchmarks.Util;

public class Program
{
    static void Main(string[] args)
    {
        var config = UserRequester.Request();
        RunHelper.Run(config);
    }
}
