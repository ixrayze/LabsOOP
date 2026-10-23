using ClinicApp.Enums;
using ClinicApp.Utils;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;

    private int _durationMinutes;
    private bool _isPaid;

    public int Id { get; }

    public int PatientId { get; }

    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    public AppointmentStatus Status { get; private set; }
    
    public bool IsCancelled =>
        Status == AppointmentStatus.Cancelled;

    public string CancellationReason =>
        IsCancelled ? Notes : "";

    public string Notes { get; private set; }
    
    public bool IsPaid => _isPaid;

    public decimal GetCost()
    {
        return DurationMinutes * 10m;
    }

    public void MarkPaid()
    {
        if (IsCancelled)
        {
            return;
        }

        _isPaid = true;
    }

    public DateTime EndsAt
    {
        get
        {
            return ScheduledAt.AddMinutes(DurationMinutes);
        }
    }

    public bool IsUpcoming
    {
        get
        {
            return ScheduledAt > DateTime.Now &&
                   Status == AppointmentStatus.Scheduled;
        }
    }

    public Appointment(
        int patientId,
        int doctorId,
        DateTime scheduledAt,
        int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;

        Status = AppointmentStatus.Scheduled;
        Notes = "";

        Id = _nextId;
        _nextId++;
    }

    public bool Cancel(string reason = "")
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            return false;
        }

        Status = AppointmentStatus.Cancelled;
        Notes = reason;

        return true;
    }

    public bool Complete()
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            return false;
        }

        Status = AppointmentStatus.Completed;
        return true;
    }

    public override string ToString()
    {
        string result =
            $"[{Id}] Пацієнт #{PatientId} → Лікар #{DoctorId} | " +
            $"{ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";

        if (Notes.Length > 0)
        {
            result += $" | {Notes}";
        }

        return result;
    }
}