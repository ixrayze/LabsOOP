namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }

    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;

        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine();
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");

        Appointment[] appointments = Appointments.GetByDate(date);

        Appointments.DisplayList(appointments);
    }

    public void GenerateReport()
    {
        Console.WriteLine();
        Console.WriteLine($"╔════════════════════════════════════╗");
        Console.WriteLine($"║ Звіт — {Name}");
        Console.WriteLine($"╠════════════════════════════════════╣");
        Console.WriteLine($"║ Пацієнтів:       {Patients.Count}");
        Console.WriteLine($"║ Лікарів:         {Doctors.Count}");

        Appointment[] upcoming = Appointments.GetUpcoming();

        Console.WriteLine($"║ Майбутніх записів: {upcoming.Length}");
        Console.WriteLine($"╠════════════════════════════════════╣");
        Console.WriteLine($"║ Навантаження лікарів (майбутні записи):");

        Doctor[] doctors = Doctors.GetAll();

        for (int i = 0; i < doctors.Length; i++)
        {
            int count = 0;

            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == doctors[i].Id)
                {
                    count++;
                }
            }

            Console.WriteLine(
                $"║   {doctors[i].FullName} ({doctors[i].Speciality}): {count} записів");
        }

        Console.WriteLine($"╚════════════════════════════════════╝");
    }
}