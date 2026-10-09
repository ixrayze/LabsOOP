using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhoneRegex =
        new Regex(@"\A[0-9]{10}\z");

    private static readonly Regex EmailRegex =
        new Regex(@"\A[^@\s]+@[^@\s]+\.[^@\s]+\z");

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException(
                "Значення не може бути порожнім або довшим за 50 символів.",
                fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) ||
            !PhoneRegex.IsMatch(phone))
        {
            throw new ArgumentException(
                "Телефон має містити рівно 10 цифр.",
                nameof(phone));
        }
    }

    public static void ValidateDate(
        DateTime value,
        string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Дата має бути не в майбутньому і не раніше 1900 року.");
        }
    }

    public static void ValidatePositive(
        int value,
        string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                "Значення має бути більшим за 0.");
        }
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        if (!EmailRegex.IsMatch(email))
        {
            throw new ArgumentException(
                "Некоректний формат email.",
                nameof(email));
        }
    }
}