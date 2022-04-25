using System;

public static class KataRound
{
    private static double Solution(double n)
    {
        double c = Math.Floor(n);
        double a = Math.Round(n - c, 1);
        double b = 0;
        if (0 < a && a < 0.3)
            b = 0;
        else if (0.2 < a && a < 0.8)
            b = 0.5;
        else if (0.7 < a && a < 1.1)
            b = 1;

        return Math.Floor(n) + b;
    }

    private static void TestCmd(double n)
    {
        Console.WriteLine($"{n}: {Solution(n)}");
    }

    public static void TestSolution()
    {
        // TestCmd(4);
        // TestCmd(4.0);
        // TestCmd(4.1);
        // TestCmd(4.2);
        // TestCmd(4.3);
        // TestCmd(4.4);
        // TestCmd(4.5);
        // TestCmd(4.6);
        // TestCmd(4.7);
        // TestCmd(4.8);
        // TestCmd(4.9);
        // TestCmd(5.0);
        // TestCmd(5);

        TestCmd(91.6994916702153);
    }
}