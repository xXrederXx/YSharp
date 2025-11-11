using System.Diagnostics;
using Xunit;
using YSharp.Types.Common;
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
        Assert.Equal(new InvalidSyntaxError(Position.Null, "").ErrorCode, res.Item2.ErrorCode);
    }
}
