namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;

    private Doctor[] _doctors = new Doctor[MaxDoctors];

    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Досягнуто ліміт лікарів.");
            return;
        }

        _doctors[_count] = doctor;
        _count++;

        Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }

        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        string search = speciality.ToLower();

        int foundCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                foundCount++;
            }
        }

        Doctor[] result = new Doctor[foundCount];

        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                result[index] = _doctors[i];
                index++;
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] result = new Doctor[_count];

        for (int i = 0; i < _count; i++)
        {
            result[i] = _doctors[i];
        }

        return result;
    }

    public bool Remove(int id)
    {
        int index = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            return false;
        }

        for (int i = index; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        int availableNow = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableNow++;
            }
        }

        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine($"Всього:         {_count}");
        Console.WriteLine($"Доступні зараз: {availableNow}");
        Console.WriteLine("По спеціальностях:");

        for (int i = 0; i < _count; i++)
        {
            bool alreadyPrinted = false;

            for (int j = 0; j < i; j++)
            {
                if (_doctors[j].Speciality.ToLower() ==
                    _doctors[i].Speciality.ToLower())
                {
                    alreadyPrinted = true;
                    break;
                }
            }

            if (alreadyPrinted)
            {
                continue;
            }

            int specialityCount = 0;

            for (int j = 0; j < _count; j++)
            {
                if (_doctors[j].Speciality.ToLower() ==
                    _doctors[i].Speciality.ToLower())
                {
                    specialityCount++;
                }
            }

            Console.WriteLine(
                $"  {_doctors[i].Speciality}: {specialityCount}");
        }

        Console.WriteLine("========================");
    }
}