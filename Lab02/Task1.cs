namespace Lab02;

public class Task1
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        double[] weights = new double[n];

        for (int i = 0; i < n; i++)
        {
            weights[i] = double.Parse(Console.ReadLine()!);
        }

        double sum = 0;
        double min = weights[0];
        double max = weights[0];

        foreach (double w in weights)
        {
            sum += w;
            if (w < min) min = w;
            if (w > max) max = w;
        }

        double avg = sum / n;

        int aboveCount = 0;
        foreach (double w in weights)
        {
            if (w > avg)
            {
                aboveCount++;
            }
        }

        Console.WriteLine($"Кількість: {n} / Середня вага: {avg:F1} кг / Мін / Макс: {min:F1} / {max:F1} кг / Вище середнього: {aboveCount} з {n}");
    }
}