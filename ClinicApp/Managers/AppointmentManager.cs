
using ClinicApp.Models;
using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;

    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _appointments[index];
        }
    }

    public AppointmentManager(
        PatientManager patients,
        DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        int durationMinutes)
    {
        Appointment appointment = new RegularAppointment(
            patientId,
            doctorId,
            scheduledAt,
            durationMinutes);

        return AddAppointment(appointment);
    }

    public bool BookUrgent(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        string urgencyNote = "",
        int durationMinutes = 30)
    {
        Appointment appointment = new UrgentAppointment(
            patientId,
            doctorId,
            scheduledAt,
            urgencyNote,
            durationMinutes);

        return AddAppointment(appointment);
    }

    public bool BookSpecialist(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        string topic = "",
        int durationMinutes = 45)
    {
        Appointment appointment = new SpecialistAppointment(
            patientId,
            doctorId,
            scheduledAt,
            topic,
            durationMinutes);

        return AddAppointment(appointment);
    }

    private bool AddAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);

        if (patient == null)
        {
            Console.WriteLine(
                $"Помилка: пацієнта з ID {appointment.PatientId} не знайдено.");

            return false;
        }

        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        if (doctor == null)
        {
            Console.WriteLine(
                $"Помилка: лікаря з ID {appointment.DoctorId} не знайдено.");

            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Досягнуто ліміт записів.");
            return false;
        }

        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine(
            $"Запис [{appointment.Id}] створено: " +
            $"{patient.FullName} → {doctor.FullName} о " +
            $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}");

        return true;
    }

    public bool Cancel(int id, string reason)
    {
        Appointment? appointment = FindById(id);

        if (appointment == null)
        {
            return false;
        }

        return appointment.Cancel(reason);
    }

    public bool Complete(int id)
    {
        Appointment? appointment = FindById(id);

        if (appointment == null)
        {
            return false;
        }

        return appointment.Complete();
    }

    public static int CancelAll(
        ICancellable[] items,
        string reason = "")
    {
        int cancelledCount = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Cancel(reason))
            {
                cancelledCount++;
            }
        }

        return cancelledCount;
    }

    public Appointment[] GetAll()
    {
        Appointment[] result = new Appointment[_count];

        for (int i = 0; i < _count; i++)
        {
            result[i] = _appointments[i];
        }

        return result;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];

        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];

        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];

        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        DateTime date = new DateTime(year, month, day);
        return GetByDate(date);
    }

    public Appointment[] GetUpcoming()
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];

        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetUrgent()
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetSpecialist()
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetRegular()
    {
        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                foundCount++;
            }
        }

        Appointment[] result = new Appointment[foundCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient =
            _patients.FindById(appointment.PatientId);

        Doctor? doctor =
            _doctors.FindById(appointment.DoctorId);

        string patientName;

        if (patient != null)
        {
            patientName = patient.FullName;
        }
        else
        {
            patientName = $"Пацієнт #{appointment.PatientId}";
        }

        string doctorName;

        if (doctor != null)
        {
            doctorName = doctor.FullName;
        }
        else
        {
            doctorName = $"Лікар #{appointment.DoctorId}";
        }

        Console.Write(
            $"[{appointment.Id}] {appointment.GetDescription()} | " +
            $"{patientName} → {doctorName} | " +
            $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}" +
            $"–{appointment.EndsAt:HH:mm} | " +
            $"{appointment.Status}");

        if (appointment.Notes.Length > 0)
        {
            Console.Write($" | {appointment.Notes}");
        }

        Console.WriteLine($" | {appointment.GetCost():F2} грн");
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }

        return null;
    }
}
