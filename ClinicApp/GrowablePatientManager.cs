namespace ClinicApp;

public class GrowablePatientManager
{
    private Patient[] _patients;
    private int _count;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public int Capacity
    {
        get
        {
            return _patients.Length;
        }
    }

    public GrowablePatientManager()
    {
        _patients = new Patient[4];
        _count = 0;
    }

    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;

        Patient[] newPatients = new Patient[newCapacity];

        for (int i = 0; i < _count; i++)
        {
            newPatients[i] = _patients[i];
        }

        _patients = newPatients;

        Console.WriteLine(
            $"Масив заповнений! Розширення: {oldCapacity} -> {newCapacity}");
    }

    public void Add(Patient patient)
    {
        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count] = patient;
        _count++;
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }

    public bool Remove(int id)
    {
        int index = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
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
            _patients[i] = _patients[i + 1];
        }

        _count--;
        
        _patients[_count] = null!;

        return true;
    }

    public void DisplayAll()
    {
        Console.WriteLine(
            $"=== Пацієнти ({Count} / {Capacity}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }
}