using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models;

/// <summary>
/// Student record: stores the three subjects the student is enrolled in.
/// Inherits common fields from Person (Id/Name/Telephone/Email/Role).
/// </summary>
public sealed class Student : Person
{
    /// <summary>
    /// Names of three subjects for the student role.
    /// </summary>
    public string Subject1 { get; set; } = string.Empty;
    public string Subject2 { get; set; } = string.Empty;
    public string Subject3 { get; set; } = string.Empty;

    /// <summary>
    /// Constructor fixes the role to Student by calling the base constructor.
    /// </summary>
    public Student()
        : base(PersonRole.Student)
    {
    }

    /// <summary>
    /// Overrides the base display to include student-specific subjects (polymorphism).
    /// </summary>
    public override string GetDetails()
    {
        return $"{base.GetDetails()} | Subjects: {Subject1}, {Subject2}, {Subject3}";
    }

    /// <summary>
    /// Short representation used by UI components (e.g., list boxes).
    /// </summary>
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }

    /// <summary>
    /// Student specific fields populated from creation request.
    /// </summary>
    public override void MapFromCreateRequest(CreatePersonRequest request)
    {
        base.MapFromCreateRequest(request);
        Subject1 = request.Subject1;
        Subject2 = request.Subject2;
        Subject3 = request.Subject3;
    }

    /// <summary>
    /// Student specific fields updated from update request.
    /// </summary>
    public override void MapFromUpdateRequest(UpdatePersonRequest request)
    {
        base.MapFromUpdateRequest(request);
        if (!string.IsNullOrWhiteSpace(request.Subject1)) Subject1 = request.Subject1;
        if (!string.IsNullOrWhiteSpace(request.Subject2)) Subject2 = request.Subject2;
        if (!string.IsNullOrWhiteSpace(request.Subject3)) Subject3 = request.Subject3;
    }
}
