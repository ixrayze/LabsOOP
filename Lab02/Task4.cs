namespace Lab02;

public class Task4
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int m = int.Parse(Console.ReadLine()!);

        int[,] matrix = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < m; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        int maxVal = matrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;

        for (int i = 0; i < n; i++)
        {
            int rowSum = 0;
            for (int j = 0; j < m; j++)
            {
                rowSum += matrix[i, j];
                if (matrix[i, j] > maxVal)
                {
                    maxVal = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
            Console.WriteLine($"Лікар {i + 1}: {rowSum} прийомів");
        }

        int[] colSums = new int[m];
        for (int j = 0; j < m; j++)
        {
            int cSum = 0;
            for (int i = 0; i < n; i++)
            {
                cSum += matrix[i, j];
            }
            colSums[j] = cSum;
        }

        Console.WriteLine($"По днях: {string.Join(", ", colSums)}");
        Console.WriteLine($"Максимум: {maxVal} (Лікар {maxRow + 1}, День {maxCol + 1})");
    }
}