namespace ClinicApp;

public class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        PatientManager patients = new PatientManager();
        DoctorManager doctors = new DoctorManager();
        AppointmentManager appointments =
            new AppointmentManager(patients, doctors);

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

        patients.Add(new Patient(
            "Марія",
            "Ткач"));

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

        doctors.Add(doctor1);
        doctors.Add(doctor2);
        doctors.Add(doctor3);

        appointments.Book(
            1,
            1,
            new DateTime(2026, 5, 9, 10, 0, 0),
            30);

        appointments.Book(
            2,
            2,
            new DateTime(2026, 5, 9, 11, 0, 0),
            45);

        appointments.Book(
            3,
            3,
            new DateTime(2026, 5, 10, 9, 0, 0),
            20);
        

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("0. Вихід");

            string choice = Console.ReadLine()!;

            if (choice == "0")
            {
                break;
            }

            if (choice == "1")
            {
                PatientsMenu(patients);
            }
            else if (choice == "2")
            {
                DoctorsMenu(doctors);
            }
            else if (choice == "3")
            {
                AppointmentsMenu(
                    appointments,
                    patients,
                    doctors);
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
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

    public static void DoctorsMenu(DoctorManager doctors)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Лікарі ===");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine("3. Знайти за спеціальністю");
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
                doctors.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;

                Console.Write("Спеціальність: ");
                string speciality = Console.ReadLine()!;

                Console.Write("Номер ліцензії: ");
                string licenseNumber = Console.ReadLine()!;

                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;

                Doctor doctor = new Doctor(
                    firstName,
                    lastName,
                    speciality,
                    licenseNumber,
                    phone);

                doctors.Add(doctor);
            }
            else if (choice == "3")
            {
                Console.Write("Введіть спеціальність: ");
                string speciality = Console.ReadLine()!;

                Doctor[] found = doctors.FindBySpeciality(speciality);

                if (found.Length == 0)
                {
                    Console.WriteLine("Лікарів не знайдено.");
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

                bool removed = doctors.Remove(id);

                if (removed)
                {
                    Console.WriteLine("Лікаря видалено.");
                }
                else
                {
                    Console.WriteLine("Лікаря не знайдено.");
                }
            }
            else if (choice == "5")
            {
                doctors.DisplayStats();
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }
    public static void AppointmentsMenu(
    AppointmentManager appointments,
    PatientManager patients,
    DoctorManager doctors)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Записи ===");
        Console.WriteLine("1. Майбутні записи");
        Console.WriteLine("2. Створити запис");
        Console.WriteLine("3. Записи пацієнта");
        Console.WriteLine("4. Записи лікаря");
        Console.WriteLine("5. Записи за датою");
        Console.WriteLine("6. Скасувати запис");
        Console.WriteLine("7. Завершити запис");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        if (choice == "0")
        {
            break;
        }

        if (choice == "1")
        {
            Appointment[] found =
                appointments.GetUpcoming();

            appointments.DisplayList(found);
        }
        else if (choice == "2")
        {
            Console.WriteLine();
            Console.WriteLine("Доступні пацієнти:");
            patients.DisplayAll();

            Console.WriteLine();
            Console.WriteLine("Доступні лікарі:");
            doctors.DisplayAll();

            Console.Write("ID пацієнта: ");
            int patientId =
                int.Parse(Console.ReadLine()!);

            Console.Write("ID лікаря: ");
            int doctorId =
                int.Parse(Console.ReadLine()!);

            Console.Write("Рік: ");
            int year =
                int.Parse(Console.ReadLine()!);

            Console.Write("Місяць: ");
            int month =
                int.Parse(Console.ReadLine()!);

            Console.Write("День: ");
            int day =
                int.Parse(Console.ReadLine()!);

            Console.Write("Година: ");
            int hour =
                int.Parse(Console.ReadLine()!);

            Console.Write("Хвилина: ");
            int minute =
                int.Parse(Console.ReadLine()!);

            Console.Write("Тривалість у хвилинах: ");
            int duration =
                int.Parse(Console.ReadLine()!);

            DateTime scheduledAt =
                new DateTime(
                    year,
                    month,
                    day,
                    hour,
                    minute,
                    0);

            appointments.Book(
                patientId,
                doctorId,
                scheduledAt,
                duration);
        }
        else if (choice == "3")
        {
            patients.DisplayAll();

            Console.Write("ID пацієнта: ");
            int patientId =
                int.Parse(Console.ReadLine()!);

            Appointment[] found =
                appointments.GetByPatient(patientId);

            appointments.DisplayList(found);
        }
        else if (choice == "4")
        {
            doctors.DisplayAll();

            Console.Write("ID лікаря: ");
            int doctorId =
                int.Parse(Console.ReadLine()!);

            Appointment[] found =
                appointments.GetByDoctor(doctorId);

            appointments.DisplayList(found);
        }
        else if (choice == "5")
        {
            Console.Write("Рік: ");
            int year =
                int.Parse(Console.ReadLine()!);

            Console.Write("Місяць: ");
            int month =
                int.Parse(Console.ReadLine()!);

            Console.Write("День: ");
            int day =
                int.Parse(Console.ReadLine()!);

            DateTime date =
                new DateTime(year, month, day);

            Appointment[] found =
                appointments.GetByDate(date);

            appointments.DisplayList(found);
        }
        else if (choice == "6")
        {
            Console.Write("ID запису: ");
            int id =
                int.Parse(Console.ReadLine()!);

            Console.Write("Причина скасування: ");
            string reason =
                Console.ReadLine()!;

            bool cancelled =
                appointments.Cancel(id, reason);

            if (cancelled)
            {
                Console.WriteLine("Запис скасовано.");
            }
            else
            {
                Console.WriteLine(
                    "Запис не знайдено або його вже завершено/скасовано.");
            }
        }
        else if (choice == "7")
        {
            Console.Write("ID запису: ");
            int id =
                int.Parse(Console.ReadLine()!);

            bool completed =
                appointments.Complete(id);

            if (completed)
            {
                Console.WriteLine("Запис завершено.");
            }
            else
            {
                Console.WriteLine(
                    "Запис не знайдено або його вже завершено/скасовано.");
            }
        }
        else
        {
            Console.WriteLine(
                "Невідомий пункт меню.");
        }
    }
}
}