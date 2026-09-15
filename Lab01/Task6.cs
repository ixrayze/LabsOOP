namespace Lab01;

public class Task6
{
    public static void Run()
    {
        Console.Write("Введіть номер картки: ");
        int card = int.Parse(Console.ReadLine()!);

        int lastDigit = card % 10;
        string department = lastDigit switch
        {
            0 or 1 => "загальна терапія",
            2 or 3 => "хірургія",
            4 or 5 => "кардіологія",
            6 or 7 => "неврологія",
            8 or 9 => "офтальмологія",
            _ => "невідомо"
        };

        string discount = (card % 2 == 0) ? "так" : "ні";
        string checkup = (card % 3 == 0) ? "так" : "ні";

        Console.WriteLine($"відділення: {department}");
        Console.WriteLine($"пільгова картка: {discount}");
        Console.WriteLine($"черговий огляд: {checkup}");
    }
}