namespace Lab01;

using System.Globalization;

public class Task1
{
    public static void Run()
    {
        Console.WriteLine("Введіть вагу пацієнта(кг): ");
        double weight = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        Console.WriteLine("Введіть зріст пацієта(м): ");
        double height = double. Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        double imt = weight /(height * height);
        Console.WriteLine($"ІМТ: {imt:F2}");
    }
}