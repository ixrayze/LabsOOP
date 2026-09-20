namespace Lab02;

public class Task6
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int[][] jagged = new int[n][];

        for (int i = 0; i < n; i++)
        {
            int k = int.Parse(Console.ReadLine()!);
            jagged[i] = new int[k];
            for (int j = 0; j < k; j++)
            {
                jagged[i][j] = int.Parse(Console.ReadLine()!);
            }
        }

        int bestDocIdx = 0;
        int maxTotal = -1;

        for (int i = 0; i < n; i++)
        {
            int k = jagged[i].Length;
            int total = 0;
            for (int j = 0; j < k; j++)
            {
                total += jagged[i][j];
            }

            double avg = k > 0 ? (double)total / k : 0.0;

            Console.WriteLine($"Лікар {i + 1}: {k} прийоми, сума={total} грн, середня={avg:F2} грн");

            if (total > maxTotal)
            {
                maxTotal = total;
                bestDocIdx = i;
            }
        }

        Console.WriteLine($"Найбільший дохід: Лікар {bestDocIdx + 1} ({maxTotal} грн)");
    }
}