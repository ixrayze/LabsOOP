using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    private IPayable[] FilterUnpaid(Appointment[] records)
    {
        int count = 0;

        for (int i = 0; i < records.Length; i++)
        {
            if (!records[i].IsPaid &&
                !records[i].IsCancelled)
            {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int index = 0;

        for (int i = 0; i < records.Length; i++)
        {
            if (!records[i].IsPaid &&
                !records[i].IsCancelled)
            {
                result[index] = records[i];
                index++;
            }
        }

        return result;
    }

    public decimal GetTotalDebt()
    {
        IPayable[] records = GetAllUnpaid();
        decimal total = 0m;

        for (int i = 0; i < records.Length; i++)
        {
            total += records[i].GetCost();
        }

        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] records = GetUnpaidByPatient(patientId);
        decimal total = 0m;

        for (int i = 0; i < records.Length; i++)
        {
            total += records[i].GetCost();
        }

        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] records = _appointments.GetAll();

        for (int i = 0; i < records.Length; i++)
        {
            if (records[i].Id == appointmentId)
            {
                if (records[i].IsPaid || records[i].IsCancelled)
                {
                    return false;
                }

                records[i].MarkPaid();
                return true;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            Console.WriteLine(
                $"{items[i]} | Сума: {items[i].GetCost():F2} грн");
        }
    }
}