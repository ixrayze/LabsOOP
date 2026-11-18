namespace ClinicApp.Attributes;

// Властивість обов'язкова: значення не може бути null або рядком з пробілів.
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredAttribute : Attribute
{
    public string ErrorMessage { get; }

    public RequiredAttribute(string errorMessage = "Field is required.")
    {
        ErrorMessage = errorMessage;
    }
}
