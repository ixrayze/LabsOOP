namespace ClinicApp;

public class Program
{
    public static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient(
            "Іван",
            "Петренко",
            new DateTime(1985, 5, 15),
            BloodType.APositive,
            "0501234567"));

        clinic.Patients.Add(new Patient(
            "Олена",
            "Коваль",
            new DateTime(1993, 8, 20),
            BloodType.BNegative,
            "0672345678"));

        clinic.Patients.Add(new Patient(
            "Максим",
            "Бойко",
            new DateTime(2010, 3, 10),
            BloodType.OPositive,
            "0933456789"));

        clinic.Patients.Add(new Patient(
            "Марія",
            "Ткач"));

        Doctor doctor1 = new Doctor(
            "Олег",
            "Сидоренко",
            Speciality.Cardiology,
            "LIC-001",
            "0441234567");

        Doctor doctor2 = new Doctor(
            "Наталія",
            "Мороз",
            Speciality.Neurology,
            "LIC-002",
            "0442345678");

        Doctor doctor3 = new Doctor(
            "Андрій",
            "Власенко",
            Speciality.Pediatrics,
            "LIC-003",
            "0443456789");

        doctor1.Schedule = new WorkSchedule(8, 16);
        doctor2.Schedule = new WorkSchedule(9, 18);
        doctor3.Schedule = new WorkSchedule(8, 17);

        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule evening = new WorkSchedule(14, 22);

        Console.WriteLine(morning);
        Console.WriteLine(evening);
        Console.WriteLine(morning.IsNow);

        WorkSchedule copy = morning;
        copy = new WorkSchedule(9, 17);

        Console.WriteLine(morning);
        Console.WriteLine(copy);

        clinic.Doctors.Add(doctor1);
        clinic.Doctors.Add(doctor2);
        clinic.Doctors.Add(doctor3);

        clinic.Appointments.Book(
            1,
            1,
            new DateTime(2026, 5, 9, 10, 0, 0),
            30);

        clinic.Appointments.Book(
            2,
            2,
            new DateTime(2026, 5, 9, 11, 0, 0),
            45);

        clinic.Appointments.Book(
            3,
            3,
            new DateTime(2026, 5, 10, 9, 0, 0),
            20);
        
        string name =
            clinic.Patients.FindById(99)?.FullName
            ?? "не знайдено";
        
        TestGrowablePatientManager();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("4. Розклад на дату");
            Console.WriteLine("5. Звіт");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            if (choice == "0")
            {
                break;
            }

            if (choice == "1")
            {
                PatientsMenu(clinic);
            }
            else if (choice == "2")
            {
                DoctorsMenu(clinic);
            }
            else if (choice == "3")
            {
                AppointmentsMenu(clinic);
            }
            else if (choice == "4")
            {
                Console.Write("Рік: ");
                int year = int.Parse(Console.ReadLine()!);

                Console.Write("Місяць: ");
                int month = int.Parse(Console.ReadLine()!);

                Console.Write("День: ");
                int day = int.Parse(Console.ReadLine()!);

                DateTime date =
                    new DateTime(year, month, day);

                clinic.DisplaySchedule(date);
            }
            else if (choice == "5")
            {
                clinic.GenerateReport();
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
    }

    public static void TestGrowablePatientManager()
    {
        Console.WriteLine();
        Console.WriteLine(
            "=== Тест GrowablePatientManager ===");

        GrowablePatientManager manager =
            new GrowablePatientManager();

        Console.WriteLine(
            "Додаємо пацієнтів одного за одним...");

        int firstId = 0;

        for (int i = 1; i <= 20; i++)
        {
            Patient patient =
                new Patient(
                    "Тест",
                    $"Пацієнт{i}");

            manager.Add(patient);

            if (i == 1)
            {
                firstId = patient.Id;
            }

            Console.WriteLine(
                $"Додано [{patient.Id}]. Розмір: {manager.Count} / {manager.Capacity}");
        }

        Console.WriteLine();
        Console.WriteLine("Тест пошуку:");

        int existingId = firstId + 9;

        Patient? found =
            manager.FindById(existingId);

        if (found != null)
        {
            Console.WriteLine(
                $"FindById({existingId}) -> {found.FullName}");
        }
        else
        {
            Console.WriteLine(
                $"FindById({existingId}) -> не знайдено");
        }

        int missingId = 999;

        Patient? missing =
            manager.FindById(missingId);

        if (missing != null)
        {
            Console.WriteLine(
                $"FindById({missingId}) -> {missing.FullName}");
        }
        else
        {
            Console.WriteLine(
                $"FindById({missingId}) -> не знайдено");
        }

        Console.WriteLine();
        Console.WriteLine("Порівняння:");
        Console.WriteLine(
            "PatientManager:         100 місць (фіксовано)");
        Console.WriteLine(
            $"GrowablePatientManager: {manager.Capacity} місць (зросте при потребі)");

        Console.WriteLine();
    }

    public static void PatientsMenu(Clinic clinic)
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
                clinic.Patients.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;

                Console.Write("Рік народження: ");
                int year =
                    int.Parse(Console.ReadLine()!);

                Console.Write("Місяць народження: ");
                int month =
                    int.Parse(Console.ReadLine()!);

                Console.Write("День народження: ");
                int day =
                    int.Parse(Console.ReadLine()!);

                Console.WriteLine("Група крові:");
                Console.WriteLine("0. Unknown");
                Console.WriteLine("1. APositive");
                Console.WriteLine("2. ANegative");
                Console.WriteLine("3. BPositive");
                Console.WriteLine("4. BNegative");
                Console.WriteLine("5. ABPositive");
                Console.WriteLine("6. ABNegative");
                Console.WriteLine("7. OPositive");
                Console.WriteLine("8. ONegative");
                Console.Write("Ваш вибір: ");

                int bloodTypeNumber =
                    int.Parse(Console.ReadLine()!);

                BloodType bloodType =
                    (BloodType)bloodTypeNumber;

                Console.Write("Телефон: ");
                string phone =
                    Console.ReadLine()!;

                Patient patient =
                    new Patient(
                        firstName,
                        lastName,
                        new DateTime(
                            year,
                            month,
                            day),
                        bloodType,
                        phone);

                clinic.Patients.Add(patient);
            }
            else if (choice == "3")
            {
                Console.Write(
                    "Введіть ім'я або прізвище: ");

                string name =
                    Console.ReadLine()!;

                Patient[] found =
                    clinic.Patients.FindByName(name);

                if (found.Length == 0)
                {
                    Console.WriteLine(
                        "Пацієнтів не знайдено.");
                }
                else
                {
                    for (int i = 0;
                         i < found.Length;
                         i++)
                    {
                        Console.WriteLine(
                            found[i]);
                    }
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID: ");

                int id =
                    int.Parse(Console.ReadLine()!);

                bool removed =
                    clinic.Patients.Remove(id);

                if (removed)
                {
                    Console.WriteLine(
                        "Пацієнта видалено.");
                }
                else
                {
                    Console.WriteLine(
                        "Пацієнта не знайдено.");
                }
            }
            else if (choice == "5")
            {
                clinic.Patients.DisplayStats();
            }
            else
            {
                Console.WriteLine(
                    "Невідомий пункт меню.");
            }
        }
    }

    public static void DoctorsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Лікарі ===");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine(
                "3. Знайти за спеціальністю");
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
                clinic.Doctors.DisplayAll();
            }
            else if (choice == "2")
            {
                Console.Write("Ім'я: ");
                string firstName =
                    Console.ReadLine()!;

                Console.Write("Прізвище: ");
                string lastName =
                    Console.ReadLine()!;

                Console.WriteLine("Спеціальність:");
                Console.WriteLine("0. General");
                Console.WriteLine("1. Cardiology");
                Console.WriteLine("2. Neurology");
                Console.WriteLine("3. Pediatrics");
                Console.WriteLine("4. Surgery");
                Console.WriteLine("5. Orthopedics");
                Console.WriteLine("6. Dermatology");
                Console.WriteLine("7. Emergency");
                Console.Write("Ваш вибір: ");

                int specialityNumber =
                    int.Parse(Console.ReadLine()!);

                Speciality speciality =
                    (Speciality)specialityNumber;

                Console.Write("Номер ліцензії: ");
                string licenseNumber =
                    Console.ReadLine()!;

                Console.Write("Телефон: ");
                string phone =
                    Console.ReadLine()!;

                Doctor doctor =
                    new Doctor(
                        firstName,
                        lastName,
                        speciality,
                        licenseNumber,
                        phone);

                clinic.Doctors.Add(doctor);
            }
            else if (choice == "3")
            {
                Console.Write(
                    "Введіть спеціальність: ");

                string speciality =
                    Console.ReadLine()!;

                Doctor[] found =
                    clinic.Doctors
                        .FindBySpeciality(
                            speciality);

                if (found.Length == 0)
                {
                    Console.WriteLine(
                        "Лікарів не знайдено.");
                }
                else
                {
                    for (int i = 0;
                         i < found.Length;
                         i++)
                    {
                        Console.WriteLine(
                            found[i]);
                    }
                }
            }
            else if (choice == "4")
            {
                Console.Write("Введіть ID: ");

                int id =
                    int.Parse(Console.ReadLine()!);

                bool removed =
                    clinic.Doctors.Remove(id);

                if (removed)
                {
                    Console.WriteLine(
                        "Лікаря видалено.");
                }
                else
                {
                    Console.WriteLine(
                        "Лікаря не знайдено.");
                }
            }
            else if (choice == "5")
            {
                clinic.Doctors.DisplayStats();
            }
            else
            {
                Console.WriteLine(
                    "Невідомий пункт меню.");
            }
        }
    }

    public static void AppointmentsMenu(
        Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Записи ===");
            Console.WriteLine(
                "1. Майбутні записи");
            Console.WriteLine(
                "2. Створити запис");
            Console.WriteLine(
                "3. Записи пацієнта");
            Console.WriteLine(
                "4. Записи лікаря");
            Console.WriteLine(
                "5. Записи за датою");
            Console.WriteLine(
                "6. Скасувати запис");
            Console.WriteLine(
                "7. Завершити запис");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice =
                Console.ReadLine()!;

            if (choice == "0")
            {
                break;
            }

            if (choice == "1")
            {
                Appointment[] found =
                    clinic.Appointments
                        .GetUpcoming();

                clinic.Appointments
                    .DisplayList(found);
            }
            else if (choice == "2")
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Доступні пацієнти:");

                clinic.Patients.DisplayAll();

                Console.WriteLine();
                Console.WriteLine(
                    "Доступні лікарі:");

                clinic.Doctors.DisplayAll();

                Console.Write(
                    "ID пацієнта: ");

                int patientId =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write(
                    "ID лікаря: ");

                int doctorId =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("Рік: ");
                int year =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("Місяць: ");
                int month =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("День: ");
                int day =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("Година: ");
                int hour =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("Хвилина: ");
                int minute =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write(
                    "Тривалість у хвилинах: ");

                int duration =
                    int.Parse(
                        Console.ReadLine()!);

                DateTime scheduledAt =
                    new DateTime(
                        year,
                        month,
                        day,
                        hour,
                        minute,
                        0);

                clinic.Appointments.Book(
                    patientId,
                    doctorId,
                    scheduledAt,
                    duration);
            }
            else if (choice == "3")
            {
                clinic.Patients.DisplayAll();

                Console.Write(
                    "ID пацієнта: ");

                int patientId =
                    int.Parse(
                        Console.ReadLine()!);

                Appointment[] found =
                    clinic.Appointments
                        .GetByPatient(
                            patientId);

                clinic.Appointments
                    .DisplayList(found);
            }
            else if (choice == "4")
            {
                clinic.Doctors.DisplayAll();

                Console.Write(
                    "ID лікаря: ");

                int doctorId =
                    int.Parse(
                        Console.ReadLine()!);

                Appointment[] found =
                    clinic.Appointments
                        .GetByDoctor(doctorId);

                clinic.Appointments
                    .DisplayList(found);
            }
            else if (choice == "5")
            {
                Console.Write("Рік: ");
                int year =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("Місяць: ");
                int month =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write("День: ");
                int day =
                    int.Parse(
                        Console.ReadLine()!);

                Appointment[] found =
                    clinic.Appointments
                        .GetByDate(
                            year,
                            month,
                            day);

                clinic.Appointments
                    .DisplayList(found);
            }
            else if (choice == "6")
            {
                Console.Write(
                    "ID запису: ");

                int id =
                    int.Parse(
                        Console.ReadLine()!);

                Console.Write(
                    "Причина скасування: ");

                string reason =
                    Console.ReadLine()!;

                bool cancelled =
                    clinic.Appointments
                        .Cancel(id, reason);

                if (cancelled)
                {
                    Console.WriteLine(
                        "Запис скасовано.");
                }
                else
                {
                    Console.WriteLine(
                        "Запис не знайдено або його вже завершено/скасовано.");
                }
            }
            else if (choice == "7")
            {
                Console.Write(
                    "ID запису: ");

                int id =
                    int.Parse(
                        Console.ReadLine()!);

                bool completed =
                    clinic.Appointments
                        .Complete(id);

                if (completed)
                {
                    Console.WriteLine(
                        "Запис завершено.");
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