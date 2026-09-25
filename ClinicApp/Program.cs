namespace ClinicApp;

public class Program
{
    public static void Main()
    {
        Doctor doctor1 = new Doctor(
            "Олег",
            "Сидоренко",
            "Кардіологія",
            "LIC-001",
            "0441234567");

        Doctor doctor2 = new Doctor(
            "Наталія",
            "Мороз",
            "Неврологія",
            "LIC-002",
            "0442345678");

        Doctor doctor3 = new Doctor(
            "Андрій",
            "Власенко",
            "Педіатрія",
            "LIC-003",
            "0443456789");

        doctor1.WorkStartHour = 8;
        doctor1.WorkEndHour = 16;

        doctor2.WorkStartHour = 9;
        doctor2.WorkEndHour = 18;

        doctor3.WorkStartHour = 8;
        doctor3.WorkEndHour = 17;

        Console.WriteLine(doctor1);
        Console.WriteLine(doctor2);
        Console.WriteLine(doctor3);
    }
}