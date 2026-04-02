using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models;

/// <summary>
/// Teaching staff record: stores salary and the names of two subjects taught.
/// </summary>
public sealed class Teacher : Person
{
    /// <summary>
    /// Salary for teaching staff.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Names of two subjects taught by the teacher role.
    /// </summary>
    public string Subject1 { get; set; } = string.Empty;
    public string Subject2 { get; set; } = string.Empty;

    /// <summary>
    /// Constructor fixes the role to Teacher by calling the base constructor.
    /// </summary>
    public Teacher()
        : base(PersonRole.Teacher)
    {
    }

    /// <summary>
    /// Overrides the base display to include salary and subjects (polymorphism).
    /// </summary>
    public override string GetDetails()
    {
        return $"{base.GetDetails()} | Salary: {Salary} | Subjects: {Subject1}, {Subject2}";
    }

    /// <summary>
    /// Short representation used by UI components.
    /// </summary>
    public override string ToString()
    {
        return $"{Id} - {Name}";
    }

    /// <summary>
    /// Teacher specific fields populated from creation request.
    /// </summary>
    public override void MapFromCreateRequest(CreatePersonRequest request)
    {
        base.MapFromCreateRequest(request);
        Salary = request.Salary;
        Subject1 = request.Subject1;
        Subject2 = request.Subject2;
    }

    /// <summary>
    /// Teacher specific fields updated from update request.
    /// </summary>
    public override void MapFromUpdateRequest(UpdatePersonRequest request)
    {
        base.MapFromUpdateRequest(request);
        if (request.Salary.HasValue) Salary = request.Salary.Value;
        if (!string.IsNullOrWhiteSpace(request.Subject1)) Subject1 = request.Subject1;
        if (!string.IsNullOrWhiteSpace(request.Subject2)) Subject2 = request.Subject2;
    }
}
