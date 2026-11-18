using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    // where T : new() — без цього обмеження new T() не скомпілюється.
    public static T Build<T>() where T : new()
    {
        T obj = new T();

        PropertyInfo[] properties = typeof(T).GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];

            // CanWrite = true і для private set, тому перевіряємо саме публічний сеттер.
            if (prop.GetSetMethod() == null)
            {
                continue;
            }

            string hint = BuildHint(prop);

            while (true)
            {
                Console.Write($"  {prop.Name}{hint}: ");

                string? input = Console.ReadLine();

                // Кінець вводу: повертаємо те, що є, — Add однаково провалідує об'єкт.
                if (input == null)
                {
                    return obj;
                }

                try
                {
                    object converted = Convert.ChangeType(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch (Exception ex) when (ex is FormatException ||
                                           ex is InvalidCastException ||
                                           ex is OverflowException)
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    // Підказка до поля збирається з його атрибутів: (обов'язкове, макс. 200, мін. 1).
    private static string BuildHint(PropertyInfo prop)
    {
        List<string> parts = new List<string>();

        if (prop.GetCustomAttribute<RequiredAttribute>() != null)
        {
            parts.Add("обов'язкове");
        }

        MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();

        if (maxLength != null)
        {
            parts.Add($"макс. {maxLength.Length}");
        }

        MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();

        if (minValue != null)
        {
            parts.Add($"мін. {minValue.Min}");
        }

        if (parts.Count == 0)
        {
            return "";
        }

        return " (" + string.Join(", ", parts) + ")";
    }
}
