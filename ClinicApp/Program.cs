using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;
using System.Globalization;
using System.Text;
using ClinicApp.Interfaces;


public class Program
{
    public static void Main()
    {
        Thread.CurrentThread.CurrentCulture =
            CultureInfo.InvariantCulture;

        Console.OutputEncoding = Encoding.UTF8;

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
        
        SeedMedicalRecords(clinic);

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
            Console.WriteLine("1. Пацієнти      — реєстрація, пошук");
            Console.WriteLine("2. Лікарі        — персонал, розклад");
            Console.WriteLine("3. Записи        — прийоми, скасування");
            Console.WriteLine("4. Медична картка — діагнози, рецепти");
            Console.WriteLine("5. Рахунки       — оплата, борги");
            Console.WriteLine("6. Звіт          — загальна статистика");
            Console.WriteLine("0. Вийти");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            try
            {
                if (choice == "0" || choice == null)
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
                    MedicalRecordsMenu(clinic);
                }
                else if (choice == "5")
                {
                    BillingMenu(clinic);
                }
                else if (choice == "6")
                {
                    clinic.GenerateReport();
                }
                else
                {
                    Console.WriteLine("Невідомий пункт меню.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Введіть число у правильному форматі.");
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "Введене число завелике.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
        }
    }
    
    public static void BillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("— Рахунки ————————————");
            Console.WriteLine("1. Борги пацієнта");
            Console.WriteLine("2. Всі неоплачені записи");
            Console.WriteLine("3. Оплатити запис");
            Console.WriteLine("4. Загальний борг клініки");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string? choice = Console.ReadLine();

            if (choice == "0" || choice == null)
            {
                return;
            }
            else if (choice == "1")
            {
                Console.Write("ID пацієнта: ");

                if (!int.TryParse(Console.ReadLine(), out int patientId))
                {
                    continue;
                }

                IPayable[] unpaid =
                    clinic.Billing.GetUnpaidByPatient(patientId);

                Console.WriteLine(
                    $"Неоплачені записи пацієнта #{patientId}:");

                clinic.Billing.DisplayUnpaid(unpaid);

                Console.WriteLine(
                    $"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
            }
            else if (choice == "2")
            {
                IPayable[] unpaid = clinic.Billing.GetAllUnpaid();

                clinic.Billing.DisplayUnpaid(unpaid);
            }
            else if (choice == "3")
            {
                Console.Write("ID запису для оплати: ");

                if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                {
                    continue;
                }

                if (clinic.Billing.PayAppointment(appointmentId))
                {
                    Console.WriteLine($"Запис [{appointmentId}] оплачено.");
                }
                else
                {
                    Console.WriteLine(
                        "Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                }
            }
            else if (choice == "4")
            {
                Console.WriteLine(
                    $"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн");
            }
        }
    }
    
    public static void MedicalRecordsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== Медична картка ===");
        Console.WriteLine("1. Записи пацієнта");
        Console.WriteLine("2. Зведення пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine()!;

        if (choice == "0" || choice == null)
        {
            break;
        }

        try
        {
            if (choice == "1")
            {
                Console.Write("ID пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);

                Patient? patient =
                    clinic.Patients.FindById(patientId);

                if (patient == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    continue;
                }

                MedicalRecord[] records =
                    clinic.MedicalRecords.GetByPatient(patientId);

                clinic.MedicalRecords.DisplayList(records);
            }
            else if (choice == "2")
            {
                Console.Write("ID пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);

                Patient? patient =
                    clinic.Patients.FindById(patientId);

                if (patient == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    continue;
                }

                clinic.MedicalRecords
                    .DisplayPatientSummary(patientId);
            }
            else if (choice == "3")
            {
                Console.Write("ID пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);

                Console.Write("ID лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);

                if (clinic.Patients.FindById(patientId) == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    continue;
                }

                if (clinic.Doctors.FindById(doctorId) == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                Console.Write("Код діагнозу: ");
                string diagnosisCode =
                    Console.ReadLine()!;

                Console.Write("Опис: ");
                string description =
                    Console.ReadLine()!;

                Console.Write("Хронічне? (1=так, 0=ні): ");
                bool isChronic =
                    Console.ReadLine() == "1";

                Diagnosis diagnosis =
                    new Diagnosis(
                        patientId,
                        doctorId,
                        DateTime.Today,
                        diagnosisCode,
                        description,
                        isChronic);

                clinic.MedicalRecords.Add(diagnosis);
            }
            else if (choice == "4")
            {
                Console.Write("ID пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);

                Console.Write("ID лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);

                if (clinic.Patients.FindById(patientId) == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    continue;
                }

                if (clinic.Doctors.FindById(doctorId) == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                Console.Write("Назва аналізу: ");
                string testName =
                    Console.ReadLine()!;

                Console.Write("Значення: ");
                string valueInput =
                    Console.ReadLine()!;

                if (!double.TryParse(
                        valueInput,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out double value))
                {
                    Console.WriteLine(
                        "Некоректне числове значення.");
                    continue;
                }

                Console.Write("Одиниці виміру: ");
                string unit =
                    Console.ReadLine()!;

                Console.Write(
                    "Норма (наприклад 4.0-9.0): ");
                string referenceRange =
                    Console.ReadLine()!;

                Console.Write("В нормі? (1=так, 0=ні): ");
                bool isNormal =
                    Console.ReadLine() == "1";

                LabResult result =
                    new LabResult(
                        patientId,
                        doctorId,
                        DateTime.Today,
                        testName,
                        value,
                        unit,
                        referenceRange,
                        isNormal);

                clinic.MedicalRecords.Add(result);
            }
            else if (choice == "5")
            {
                Console.Write("ID пацієнта: ");
                int patientId = int.Parse(Console.ReadLine()!);

                Console.Write("ID лікаря: ");
                int doctorId = int.Parse(Console.ReadLine()!);

                if (clinic.Patients.FindById(patientId) == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    continue;
                }

                if (clinic.Doctors.FindById(doctorId) == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                Console.Write("Препарат: ");
                string medicationName =
                    Console.ReadLine()!;

                Console.Write("Дозування: ");
                string dosage =
                    Console.ReadLine()!;

                Console.Write("Кількість днів: ");
                int durationDays =
                    int.Parse(Console.ReadLine()!);

                Console.Write(
                    "Інструкція (Enter — пропустити): ");
                string instructions =
                    Console.ReadLine() ?? "";

                Prescription prescription =
                    new Prescription(
                        patientId,
                        doctorId,
                        DateTime.Today,
                        medicationName,
                        dosage,
                        durationDays,
                        instructions);

                clinic.MedicalRecords.Add(prescription);
            }
            else if (choice == "6")
            {
                Console.Write("ID лікаря: ");
                int doctorId =
                    int.Parse(Console.ReadLine()!);

                Doctor? doctor =
                    clinic.Doctors.FindById(doctorId);

                if (doctor == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                MedicalRecord[] records =
                    clinic.MedicalRecords.GetByDoctor(doctorId);

                clinic.MedicalRecords.DisplayList(records);
            }
            else
            {
                Console.WriteLine("Невідомий пункт меню.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine(
                "Введіть число у правильному форматі.");
        }
        catch (OverflowException)
        {
            Console.WriteLine(
                "Введене число завелике.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(
                $"Помилка: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(
                $"Помилка: {ex.Message}");
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

            try
            {
                if (choice == "0" || choice == null)
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
                    string firstName =
                        Console.ReadLine()!;

                    Console.Write("Прізвище: ");
                    string lastName =
                        Console.ReadLine()!;

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
                    Console.Write(
                        "Введіть ID: ");

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
            catch (FormatException)
            {
                Console.WriteLine(
                    "Введіть число у правильному форматі.");
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "Введене число завелике.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
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
            Console.WriteLine("5. Вільні години лікаря");
            Console.WriteLine("6. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()!;

            try
            {
                if (choice == "0" || choice == null)
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

                    Console.Write(
                        "Номер ліцензії: ");
                    string licenseNumber =
                        Console.ReadLine()!;

                    Console.Write("Телефон: ");
                    string phone =
                        Console.ReadLine()!;

                    Console.Write(
                        "Початок робочого дня (0-23): ");
                    int workStartHour =
                        int.Parse(Console.ReadLine()!);

                    Console.Write(
                        "Кінець робочого дня (1-24): ");
                    int workEndHour =
                        int.Parse(Console.ReadLine()!);

                    Doctor doctor =
                        new Doctor(
                            firstName,
                            lastName,
                            speciality,
                            licenseNumber,
                            phone);

                    doctor.Schedule =
                        new WorkSchedule(
                            workStartHour,
                            workEndHour);

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
                    Console.Write(
                        "Введіть ID: ");

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
                    Console.Write("ID лікаря: ");

                    if (!int.TryParse(Console.ReadLine(), out int doctorId))
                    {
                        Console.WriteLine("Некоректний ID лікаря.");
                        continue;
                    }

                    Doctor? doctor = clinic.Doctors.FindById(doctorId);

                    if (doctor == null)
                    {
                        Console.WriteLine("Лікаря не знайдено.");
                        continue;
                    }

                    Console.Write("Дата (dd.MM.yyyy): ");

                    if (!DateTime.TryParseExact(
                            Console.ReadLine(),
                            "dd.MM.yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime date))
                    {
                        Console.WriteLine("Некоректна дата.");
                        continue;
                    }

                    Console.Write("Скільки слотів: ");

                    if (!int.TryParse(Console.ReadLine(), out int slotCount))
                    {
                        Console.WriteLine("Некоректна кількість слотів.");
                        continue;
                    }

                    ISchedulable schedule = doctor;

                    DateTime[] slots = schedule.GetAvailableSlots(date, slotCount);

                    for (int i = 0; i < slots.Length; i++)
                    {
                        Console.WriteLine(slots[i].ToString("HH:mm"));
                    }
                }
                else if (choice == "6")
                {
                    clinic.Doctors.DisplayStats();
                }
                else
                {
                    Console.WriteLine(
                        "Невідомий пункт меню.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Введіть число у правильному форматі.");
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "Введене число завелике.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
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
            Console.WriteLine(
                "8. Скасувати всі записи пацієнта");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш вибір: ");

            string choice =
                Console.ReadLine()!;

            try
            {
                if (choice == "0" || choice == null)
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
                else if (choice == "8")
                {
                    Console.Write("ID пацієнта: ");

                    int patientId = int.Parse(Console.ReadLine()!);

                    Console.Write("Причина (Enter — без причини): ");
                    string reason = Console.ReadLine() ?? "";

                    Appointment[] appointments =
                        clinic.Appointments.GetByPatient(patientId);

                    int cancelledCount =
                        AppointmentManager.CancelAll(appointments, reason);

                    Console.WriteLine($"Скасовано записів: {cancelledCount}");
                }
                else
                {
                    Console.WriteLine(
                        "Невідомий пункт меню.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Введіть число у правильному форматі.");
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "Введене число завелике.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"Помилка: {ex.Message}");
            }
        }
    }
    
    public static void SeedMedicalRecords(Clinic clinic)
    {
        // Пацієнт #1 — два діагнози
        clinic.MedicalRecords.Add(new Diagnosis(
            1,
            1,
            DateTime.Today.AddDays(-30),
            "I10",
            "Гіпертонічна хвороба",
            true));

        clinic.MedicalRecords.Add(new Diagnosis(
            1,
            1,
            DateTime.Today.AddDays(-5),
            "J06.9",
            "Гострий ринофарингіт"));

        // Пацієнт #1 — аналіз у нормі
        clinic.MedicalRecords.Add(new LabResult(
            1,
            1,
            DateTime.Today.AddDays(-10),
            "Гемоглобін",
            145,
            "г/л",
            "120-160",
            true));

        // Пацієнт #1 — аналіз поза нормою
        clinic.MedicalRecords.Add(new LabResult(
            1,
            1,
            DateTime.Today.AddDays(-8),
            "Холестерин",
            6.2,
            "ммоль/л",
            "< 5.2",
            false));

        // Пацієнт #1 — активний рецепт
        clinic.MedicalRecords.Add(new Prescription(
            1,
            1,
            DateTime.Today.AddDays(-5),
            "Лізиноприл",
            "10 мг",
            30,
            "1 раз на добу вранці"));

        // Пацієнт #2 — завершений рецепт
        clinic.MedicalRecords.Add(new Prescription(
            2,
            2,
            DateTime.Today.AddDays(-40),
            "Амоксицилін",
            "500 мг",
            10));

        // Записи ще для інших пацієнтів
        clinic.MedicalRecords.Add(new Diagnosis(
            2,
            2,
            DateTime.Today.AddDays(-15),
            "K29.7",
            "Гастрит"));

        clinic.MedicalRecords.Add(new LabResult(
            3,
            3,
            DateTime.Today.AddDays(-3),
            "Глюкоза",
            4.8,
            "ммоль/л",
            "3.9-5.5",
            true));
    }
}