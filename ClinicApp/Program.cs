namespace ClinicApp;

public class Program
{
    public static void Main()
    {
        PatientManager patients = new PatientManager();

        patients.Add(new Patient(
            "Іван",
            "Петренко",
            new DateTime(1985, 5, 15),
            "A+",
            "0501234567"));

        patients.Add(new Patient(
            "Олена",
            "Коваль",
            new DateTime(1993, 8, 20),
            "B-",
            "0672345678"));

        patients.Add(new Patient(
            "Максим",
            "Бойко",
            new DateTime(2010, 3, 10),
            "O+",
            "0933456789"));

        patients.Add(new Patient("Марія", "Ткач"));

        PatientsMenu(patients);
    }

    public static void PatientsMenu(PatientManager patients)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Пацієнти ===");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0")
            {
                break;
            }

            if (choice == "1")
            {
                patients.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;

                Console.Write("Рік народження: ");
                int year = int.Parse(Console.ReadLine()!);

                Console.Write("Місяць народження: ");
                int month = int.Parse(Console.ReadLine()!);

                Console.Write("День народження: ");
                int day = int.Parse(Console.ReadLine()!);

                Console.Write("Група крові: ");
                string bloodType = Console.ReadLine()!;

                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Patient patient = new Patient(
                    firstName,
                    lastName,
                    new DateTime(year, month, day),
                    bloodType,
                    phone);

                patients.Add(patient);
            }
            else if (choice == "3")
            {
                Console.Write("Введіть ім'я або прізвище: ");
                string name = Console.ReadLine()!;

                Patient[] found = patients.FindByName(name);

                if (found.Length == 0)
                {
                    Console.WriteLine("Пацієнтів не знайдено.");
                }
                else
                {
                    for (int i = 0; i < found.Length; i++)
                    {
                        Console.WriteLine(found[i]);
                    }
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID: ");
                int id = int.Parse(Console.ReadLine()!);

                bool removed = patients.Remove(id);

                if (removed)
                {
                    Console.WriteLine("Пацієнта видалено.");
                }
                else
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                }
            }
            else if (choice == "5")
            {
                patients.DisplayStats();
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }
}