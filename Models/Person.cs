using EducationCentreSystem.Common;
using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models;

/// <summary>
/// Abstract base type for all user groups in the education centre.
/// Marked as abstract to prevent creating a generic "Person" record; only specific roles
/// (Student/Teacher/Admin) should be instantiated.
/// </summary>
public abstract class Person
{
    /// <summary>
    /// Unique identifier for the record (assigned by the repository).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the person.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Telephone number stored as a normalized string.
    /// </summary>
    public string Telephone { get; set; } = string.Empty;

    /// <summary>
    /// Email address stored as a normalized string.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Role is read-only to keep the record's type consistent after creation.
    /// </summary>
    public PersonRole Role { get; }

    /// <summary>
    /// Protected constructor ensures only derived classes can set the Role.
    /// </summary>
    protected Person(PersonRole role)
    {
        Role = role;
    }

    /// <summary>
    /// Returns a formatted string for displaying the record.
    /// Declared virtual so derived classes can override and append role-specific fields (polymorphism).
    /// </summary>
    public virtual string GetDetails()
    {
        return $"ID: {Id} | Role: {Role} | Name: {Name} | Telephone: {Telephone} | Email: {Email}";
    }

    /// <summary>
    /// Populates common fields from a creation request.
    /// Derived classes must override this to populate their own specific fields.
    /// </summary>
    public virtual void MapFromCreateRequest(CreatePersonRequest request)
    {
        Name = request.Name.Trim();
        Email = ValidationHelper.NormalizeEmail(request.Email)!;
        Telephone = ValidationHelper.NormalizeTelephone(request.Telephone)!;
    }

    /// <summary>
    /// Updates common fields from an update request if they are provided.
    /// Derived classes must override this to update their own specific fields.
    /// </summary>
    public virtual void MapFromUpdateRequest(UpdatePersonRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name)) 
            Name = request.Name.Trim();

        if (!string.IsNullOrWhiteSpace(request.Telephone))
            Telephone = ValidationHelper.NormalizeTelephone(request.Telephone)!;
    }
}
