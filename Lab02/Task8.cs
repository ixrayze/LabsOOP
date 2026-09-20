namespace Lab02;

public class Task8
{
    public static void Run()
    {
        int d = int.Parse(Console.ReadLine()!);
        int w = int.Parse(Console.ReadLine()!);

        int[,,] stats = new int[d, w, 2];

        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < w; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    stats[i, j, k] = int.Parse(Console.ReadLine()!);
                }
            }
        }

        int[] deptTotals = new int[d];
        int maxDeptIdx = 0;

        for (int i = 0; i < d; i++)
        {
            Console.WriteLine($"Відділення {i + 1}:");
            int currentDeptSum = 0;

            for (int j = 0; j < w; j++)
            {
                int morning = stats[i, j, 0];
                int evening = stats[i, j, 1];
                int weekTotal = morning + evening;
                currentDeptSum += weekTotal;

                Console.WriteLine($"  Тиждень {j + 1}: ранок {morning}, вечір {evening} -> разом {weekTotal}");
            }

            deptTotals[i] = currentDeptSum;
            Console.WriteLine($"Разом: {currentDeptSum} пацієнтів");

            if (currentDeptSum > deptTotals[maxDeptIdx])
            {
                maxDeptIdx = i;
            }
        }

        Console.WriteLine($"Найзавантаженіше: Відділення {maxDeptIdx + 1} ({deptTotals[maxDeptIdx]} пацієнтів)");
    }
}