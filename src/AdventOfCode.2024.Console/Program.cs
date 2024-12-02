// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common;

var shouldExit = false;
do
{
    Console.WriteLine("Welcome to the Advent of Code, Year 2024.");

    Console.Write("Do you want to run in test mode? ([y]/n): ");
    var key = Console.ReadKey();

    var isTestMode = key.KeyChar == 'y' || key.KeyChar == 'Y' || key.Key == ConsoleKey.Enter;

    int todayDay = DateTime.Today.Day;

    string defaultDay = todayDay <= 25 ? $" [Enter for {todayDay}]" : string.Empty;

    Console.WriteLine();

    Console.Write($"Type the day you want to run (1-25){defaultDay}: ");

    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) && todayDay <= 25)
    {
        input = todayDay.ToString();
    }

    if (int.TryParse(input, out int day))
    {
        LoadAndRunDay(day, isTestMode);
    }
    else
    {
        Console.WriteLine("Invalid day.");
    }

    Console.Write("Exit? ([y]/n): ");
    key = Console.ReadKey();
    Console.WriteLine();
    Console.Clear();

    shouldExit = key.KeyChar == 'y' || key.KeyChar == 'Y' || key.Key == ConsoleKey.Enter;

} while (!shouldExit);


static void LoadAndRunDay(int day, bool isTestMode)
{
    string className = $"AdventOfCode.Day.Day{day}, AdventOfCode.Day";
    Type dayType = Type.GetType(className);
    if (dayType == null)
    {
        Console.WriteLine($"Class {className} not found.");
        // Debugging: List all types in the AdventOfCode.Day namespace
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.Namespace != null && t.Namespace == "AdventOfCode.Day");
        foreach (var type in allTypes)
        {
            Console.WriteLine($"Available type: {type.FullName}");
        }
        return;
    }

    try
    {
        var instance = Activator.CreateInstance(dayType) as AdventCalendarDay ??
             throw new Exception($"Failed to create instance of {className}");

        instance.Run(isTestMode);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Exception occurred while creating instance of {className}: {ex.Message}");
    }
}