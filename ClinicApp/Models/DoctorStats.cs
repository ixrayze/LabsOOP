namespace ClinicApp.Models;

public class DoctorStats : IComparable<DoctorStats>
{
    public int DoctorId { get; }

    public string FullName { get; }

    public int AppointmentCount { get; }

    public decimal TotalRevenue { get; }

    public DateTime LastAppointmentDate { get; }

    public DoctorStats(
        int doctorId,
        string fullName,
        int appointmentCount,
        decimal totalRevenue,
        DateTime lastAppointmentDate)
    {
        DoctorId = doctorId;
        FullName = fullName;
        AppointmentCount = appointmentCount;
        TotalRevenue = totalRevenue;
        LastAppointmentDate = lastAppointmentDate;
    }

    // Природний порядок: лікар із більшою кількістю прийомів стоїть першим,
    // тому порівнюємо навпаки — other з this.
    public int CompareTo(DoctorStats? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other.AppointmentCount.CompareTo(AppointmentCount);
    }

    public override string ToString()
    {
        string lastDate;

        if (LastAppointmentDate == DateTime.MinValue)
        {
            lastDate = "—";
        }
        else
        {
            lastDate = LastAppointmentDate.ToString("dd.MM.yyyy");
        }

        return $"[{DoctorId}] {FullName} | Прийомів: {AppointmentCount} | " +
               $"Виручка: {TotalRevenue:F2} грн | Останній: {lastDate}";
    }
}
