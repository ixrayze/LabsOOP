namespace ClinicApp;

public class Program
{
    public static void Main()
    {
        Patient patient1 = new Patient(
            "Іван",
            "Петренко",
            new DateTime(1985, 5, 15),
            "A+",
            "0501234567");

        Patient patient2 = new Patient(
            "Олена",
            "Коваль",
            new DateTime(1993, 8, 20),
            "B-",
            "0672345678");

        Patient patient3 = new Patient(
            "Максим",
            "Бойко",
            new DateTime(2010, 3, 10),
            "O+",
            "0933456789");

        Patient patient4 = new Patient();

        Patient patient5 = new Patient("Марія", "Ткач");

        Console.WriteLine(patient1);
        Console.WriteLine(patient2);
        Console.WriteLine(patient3);
        Console.WriteLine(patient4);
        Console.WriteLine(patient5);
    }
}