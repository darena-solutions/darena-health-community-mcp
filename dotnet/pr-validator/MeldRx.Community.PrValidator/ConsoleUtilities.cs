namespace MeldRx.Community.PrValidator;

public static class ConsoleUtilities
{
    public static void WriteErrorLine(string error)
    {
        var currentColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ForegroundColor = currentColor;
    }
}
