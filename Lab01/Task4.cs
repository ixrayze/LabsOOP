namespace Lab01;

public class Task4
{
    public static void Run()
    {
        Console.Write("Введіть систолічний тиск: ");
        int sys = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть діастолічний тиск: ");
        int dia = int.Parse(Console.ReadLine()!);

        string status;
        if (sys < 120 && dia < 80)
        {
            status = "норма";
        }
        else if (sys < 130 && dia < 80)
        {
            status = "підвищений";
        }
        else if (sys < 140 || dia < 90)
        {
            status = "гіпертонія 1 ступеня";
        }
        else
        {
            status = "гіпертонія 2 ступеня";
        }

        Console.WriteLine($"Тиск: {sys}/{dia} — {status}");
    }
}