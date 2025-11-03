using System.Diagnostics;
using Xunit;
using YSharp.Utils;

public class Error1xxxTest
{
    [Fact]
    public void Test_YS1000()
    {
        // Given
        string inp = "Some invalide syntax";
        RunClass runClass = new RunClass();

        // When
        var res = runClass.Run("TEST", inp);
        // Then

    }
}