using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    // Перевіряє будь-який об'єкт: правила беруться з атрибутів на його властивостях,
    // тому тут немає жодного звернення до конкретних полів.
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();

        // GetType() — фактичний тип об'єкта під час виконання.
        PropertyInfo[] properties = obj.GetType().GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];

            // Індексатори й властивості без get неможливо прочитати через GetValue(obj).
            if (!prop.CanRead || prop.GetIndexParameters().Length > 0)
            {
                continue;
            }

            object? value = prop.GetValue(obj);

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();

            if (required != null)
            {
                if (value == null ||
                    (value is string text && string.IsNullOrWhiteSpace(text)))
                {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();

            if (maxLength != null)
            {
                if (value is string text && text.Length > maxLength.Length)
                {
                    result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
                }
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();

            if (minValue != null && value != null)
            {
                if (Convert.ToDouble(value) < minValue.Min)
                {
                    result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
                }
            }
        }

        return result;
    }

    // Рефлексія працює і без екземпляра: достатньо Type.
    public static void PrintInfo(Type type)
    {
        Console.WriteLine(type.Name);

        PropertyInfo[] properties = type.GetProperties();

        int width = 0;

        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].Name.Length > width)
            {
                width = properties[i].Name.Length;
            }
        }

        for (int i = 0; i < properties.Length; i++)
        {
            string line = "  " + properties[i].Name.PadRight(width + 2);

            foreach (Attribute attribute in properties[i].GetCustomAttributes())
            {
                string name = attribute.GetType().Name;

                if (name.EndsWith("Attribute"))
                {
                    name = name.Substring(0, name.Length - "Attribute".Length);
                }

                line += $"[{name}] ";
            }

            Console.WriteLine(line.TrimEnd());
        }
    }
}
