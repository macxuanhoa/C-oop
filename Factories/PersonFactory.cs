using EducationCentreSystem.Models;

namespace EducationCentreSystem.Factories;

/// <summary>
/// Factory responsible for creating the correct derived Person type based on role.
/// This centralizes object creation and supports polymorphism by returning the base type.
/// </summary>
public static class PersonFactory
{
    /// <summary>
    /// Creates a Student, Teacher, or Admin instance depending on the given role.
    /// </summary>
    public static Person Create(PersonRole role)
    {
        return role switch
        {
            PersonRole.Student => new Student(),
            PersonRole.Teacher => new Teacher(),
            PersonRole.Admin => new Admin(),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
