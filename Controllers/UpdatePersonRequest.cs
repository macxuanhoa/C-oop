using System.Collections.Generic;
using EducationCentreSystem.Models;
using EducationCentreSystem.Common;

namespace EducationCentreSystem.Controllers;

/// <summary>
/// Input model used when updating an existing record.
/// Unlike creation, most fields are optional: blank values mean "keep current".
/// TargetEmail identifies the record to update.
/// </summary>
public sealed record UpdatePersonRequest
{
    /// <summary>
    /// Email used to locate the record to update.
    /// </summary>
    public string TargetEmail { get; init; } = string.Empty;

    /// <summary>
    /// Common fields that can be updated.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    public string Telephone { get; init; } = string.Empty;

    /// <summary>
    /// Student fields (optional when updating).
    /// </summary>
    public string Subject1 { get; init; } = string.Empty;
    public string Subject2 { get; init; } = string.Empty;
    public string Subject3 { get; init; } = string.Empty;

    /// <summary>
    /// Teacher/Admin fields (optional when updating).
    /// Null means "do not change".
    /// </summary>
    public decimal? Salary { get; init; }

    /// <summary>
    /// Admin fields (optional when updating).
    /// </summary>
    public string FullTimeOrPartTime { get; init; } = string.Empty;
    public int? WorkingHours { get; init; }

    /// <summary>
    /// Validates update input and returns a dictionary of field-to-error messages.
    /// Validation only applies to fields that are provided.
    /// </summary>
    public Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(TargetEmail)) 
            errors.Add(nameof(TargetEmail), "Target email is required.");
        else if (!ValidationHelper.IsValidEmail(TargetEmail.Trim()))
            errors.Add(nameof(TargetEmail), "Target email format is invalid.");

        if (!string.IsNullOrWhiteSpace(Telephone) && ValidationHelper.NormalizeTelephone(Telephone) == null)
            errors.Add(nameof(Telephone), "Telephone format is invalid.");

        if (!string.IsNullOrWhiteSpace(FullTimeOrPartTime) && ValidationHelper.NormalizeJobType(FullTimeOrPartTime) == null)
            errors.Add(nameof(FullTimeOrPartTime), "Job type must be Full-time or Part-time.");
        
        if (Salary.HasValue && Salary.Value <= 0) 
            errors.Add(nameof(Salary), "Salary must be greater than 0.");

        if (WorkingHours.HasValue && WorkingHours.Value <= 0)
            errors.Add(nameof(WorkingHours), "Working hours must be greater than 0.");

        return errors;
    }
}
