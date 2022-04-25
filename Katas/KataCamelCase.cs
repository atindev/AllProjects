using System;
using System.Linq;
using System.Text;

public static class KataCamelCase
{
    private static string BreakCamelCase(string str)
    {
        StringBuilder stringBuilder = new StringBuilder();
        // complete the function
        str.ToList().ForEach(x => stringBuilder.Append(Char.IsUpper(x) ? $" {x}" : $"{x}"));
        return stringBuilder.ToString();
    }

    public static void TestCamelCase()
    {
        System.Console.WriteLine(BreakCamelCase("camelCasing"));
        System.Console.WriteLine(BreakCamelCase("camelCasingTest"));
    }
}