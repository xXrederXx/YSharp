using System;
using BenchmarkDotNet.Attributes;

namespace YSharp.Benchmarks;

[SimpleJob]
[MemoryDiagnoser]
public class Bench1
{
    string text = ";FUN add(a, b);  RETURN a + b;END;;FUN forTest();    FOR x = 0 TO 10 THEN;        PRINT(x);    END;;    FOR x = 0 TO 20 STEP 2 THEN;        PRINT(x);    END;END;;FUN whileTest();    VAR x = \"cool\";    WHILE x == \"cool\" THEN;        PRINT(\"x is cool\");        IF INPUT() == \"bad\" THEN;            VAR x = \"bad\";        END;    END;END;;# this is a comment;;VAR x = \"69.42\";VAR y = [0, 9, 11];VAR time = TIME();;PRINT(y.IndexOf(11));PRINT(y.Remove(0));;add(x.ToNumber(), y.Get(0));forTest();whileTest();;PRINT(TIME() - time);FUN foo(a, b, c);    a + b;    a / c;    c * b;    MATH.SIN(b + c);END;;VAR list = [];VAR list2 = [];VAR str = \"\";FOR x = 0 TO 10_000 THEN;    list.Add(x);    VAR y = 25 * (2 / 5 + 6)^2;    VAR b = NOT TRUE AND NOT FALSE OR TRUE;    IF x > 50 THEN;        list2.Add(x);    END;    VAR str += x.ToString();    foo(x, 3, 6);END;;FUN GetCSq(a, b);    RETURN MATH.SQRT(a * a + b * b);END;;FUN RUNTEST();    FOR x = 0 TO 360 THEN;        FOR y = 0 TO 360 THEN;            (GetCSq(x, y));        END;    END;END;;FUN CountTo(x);    PRINT(\"Here is done\");    VAR zg = 0;    FOR y = 0 TO x THEN;        VAR zg = zg + y;    END;    PRINT(zg);END;;VAR zigzag = 1000;CountTo(zigzag);VAR list = [1, 2];PRINT(list.Count);TRY;    PRINT(\"Huray\" + r);END;CATCH;    PRINT(\"Cought\");END";

    public void Benchmark()
    {
        RunClass runClass = new();
        runClass.Run("BENCHMARK", text);
    }
}
