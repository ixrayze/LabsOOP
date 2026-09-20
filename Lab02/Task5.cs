namespace Lab02;

public class Task5
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int[,] matrix = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        int[] mainDiag = new int[n];
        int[] sideDiag = new int[n];
        int mainSum = 0;
        int sideSum = 0;

        for (int i = 0; i < n; i++)
        {
            int mainVal = matrix[i, i];
            int sideVal = matrix[i, n - 1 - i];

            mainDiag[i] = mainVal;
            sideDiag[i] = sideVal;

            mainSum += mainVal;
            sideSum += sideVal;
        }

        Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiag)} (сума = {mainSum})");
        Console.WriteLine($"Побічна діагональ: {string.Join(", ", sideDiag)} (сума = {sideSum})");
    }
}