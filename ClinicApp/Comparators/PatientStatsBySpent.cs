using ClinicApp.Models;

namespace ClinicApp.Comparators;

public class PatientStatsBySpent : IComparer<PatientStats>
{
    // Спадання: більші витрати вище, тому порівнюємо y з x.
    public int Compare(PatientStats? x, PatientStats? y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        return y.TotalSpent.CompareTo(x.TotalSpent);
    }
}
