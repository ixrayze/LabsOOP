namespace ClinicApp;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bloodType)
    {
        return bloodType switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            _ => "Невідомо"
        };
    }

    public static string FormatSpeciality(Speciality speciality)
    {
        return speciality switch
        {
            Speciality.General => "Загальна практика",
            Speciality.Cardiology => "Кардіологія",
            Speciality.Neurology => "Неврологія",
            Speciality.Pediatrics => "Педіатрія",
            Speciality.Surgery => "Хірургія",
            Speciality.Orthopedics => "Ортопедія",
            Speciality.Dermatology => "Дерматологія",
            Speciality.Emergency => "Невідкладна допомога",
            _ => "Невідомо"
        };
    }

    public static string FormatAge(int age)
    {
        if (age % 100 >= 11 && age % 100 <= 19)
        {
            return age + " років";
        }

        int lastDigit = age % 10;

        if (lastDigit == 1)
        {
            return age + " рік";
        }

        if (lastDigit >= 2 && lastDigit <= 4)
        {
            return age + " роки";
        }

        return age + " років";
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10)
        {
            return phone;
        }

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
            {
                return phone;
            }
        }

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}–{phone.Substring(6, 4)}";
    }
}