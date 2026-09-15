namespace Lab01;

using System.Globalization;

public class Task7
{
    public static void Run()
    {
        Console.Write("Введіть кількість прийомів: ");
        int n = int.Parse(Console.ReadLine()!);
        decimal[] costs = new decimal[n];

        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
        }

        decimal total = 0;
        decimal min = n > 0 ? costs[0] : 0;
        decimal max = n > 0 ? costs[0] : 0;

        foreach (decimal cost in costs)
        {
            total += cost;
            if (cost < min) min = cost;
            if (cost > max) max = cost;
        }

        decimal average = n > 0 ? total / n : 0;

        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if (costs[i] > average)
            {
                aboveAverage++;
            }
        }


        string firstOver1000 = "немає";
        int idx = 0;
        while (idx < n)
        {
            if (costs[idx] > 1000)
            {
                firstOver1000 = $"#{idx + 1} — {costs[idx]:F2} грн";
                break;
            }

            idx++;
        }

        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість:        {n}");
        Console.WriteLine($"Загальна сума:    {total:F2} грн");
        Console.WriteLine($"Середня:          {average:F2} грн");
        Console.WriteLine($"Мін / Макс:       {min:F2} / {max:F2} грн");
        Console.WriteLine($"Вище середнього:  {aboveAverage} з {n}");
        Console.WriteLine($"Перший > 1000:    {firstOver1000}");
        Console.WriteLine("========================");
    }
}