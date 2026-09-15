using System.Globalization;

namespace Lab01;

public class Task2
{
    public static void Run()
    {
        Console.Write("Введіть базову ціну послуги: ");
        double price = double.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        Console.Write("Введіть кількість прийомів: ");
        int count = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть розмір знижки(%): ");
        int discount = int.Parse(Console.ReadLine()!);

        double total = price * count * (1.0 - discount / 100.0);
        Console.WriteLine($"Сума: {total:F2} грн");
    }
}