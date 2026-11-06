using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count
    {
        get
        {
            return _patients.Count;
        }
    }

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
            {
                return null;
            }

            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);

        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public bool TryFindById(int id, out Patient patient)
    {
        Patient? found = FindById(id);

        if (found != null)
        {
            patient = found;
            return true;
        }

        patient = null!;
        return false;
    }

    public Patient[] FindByName(string name)
    {
        string search = name.ToLower();

        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            string firstName = _patients[i].FirstName.ToLower();
            string lastName = _patients[i].LastName.ToLower();

            if (firstName.Contains(search) || lastName.Contains(search))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");

        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        int totalAge = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;

            totalAge += age;

            if (age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }
        }

        double averageAge = (double)totalAge / _patients.Count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:          {_patients.Count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine(
            $"Наймолодший: {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine(
            $"Найстарший:  {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:        {adultCount} з {_patients.Count}");
        Console.WriteLine("========================");
    }
}