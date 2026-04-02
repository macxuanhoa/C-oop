using System.Collections.Generic;
using EducationCentreSystem.Models;
using EducationCentreSystem.Common;

namespace EducationCentreSystem.Controllers;

/// <summary>
/// Input model used when creating a new person record.
/// Includes both common fields and role-specific fields; validation enforces required fields for creation.
/// </summary>
public sealed record CreatePersonRequest
{
    /// <summary>
    /// Role determines which derived type will be created (Student/Teacher/Admin).
    /// </summary>
    public PersonRole Role { get; init; }

    /// <summary>
    /// Common fields required for all roles.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    public string Telephone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Student fields (three subjects). Only used when Role is Student.
    /// </summary>
    public string Subject1 { get; init; } = string.Empty;
    public string Subject2 { get; init; } = string.Empty;
    public string Subject3 { get; init; } = string.Empty;

    /// <summary>
    /// Teacher/Admin fields (salary). Required for Teacher and Admin.
    /// </summary>
    public decimal Salary { get; init; }

    /// <summary>
    /// Admin fields (job type and working hours). Required for Admin.
    /// </summary>
    public string FullTimeOrPartTime { get; init; } = string.Empty;
    public int WorkingHours { get; init; }

    /// <summary>
    /// Validates creation input and returns a dictionary of field-to-error messages.
    /// </summary>
    public Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(Name)) 
            errors.Add(nameof(Name), "Name is required.");

        if (string.IsNullOrWhiteSpace(Email)) 
            errors.Add(nameof(Email), "Email is required.");
        else if (!ValidationHelper.IsValidEmail(Email.Trim()))
            errors.Add(nameof(Email), "Email format is invalid.");

        if (string.IsNullOrWhiteSpace(Telephone)) 
            errors.Add(nameof(Telephone), "Telephone is required.");
        else if (ValidationHelper.NormalizeTelephone(Telephone) == null)
            errors.Add(nameof(Telephone), "Telephone format is invalid.");

        if (Role == PersonRole.Teacher || Role == PersonRole.Admin)
        {
            if (Salary <= 0) errors.Add(nameof(Salary), "Salary must be greater than 0.");
        }

        if (Role == PersonRole.Admin)
        {
            if (string.IsNullOrWhiteSpace(FullTimeOrPartTime)) 
                errors.Add(nameof(FullTimeOrPartTime), "Job type is required.");
            else if (ValidationHelper.NormalizeJobType(FullTimeOrPartTime) == null)
                errors.Add(nameof(FullTimeOrPartTime), "Job type must be Full-time or Part-time.");

            if (WorkingHours <= 0) errors.Add(nameof(WorkingHours), "Working hours must be greater than 0.");
        }

        return errors;
    }
}
