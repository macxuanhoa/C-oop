using System.Collections.Generic;
using EducationCentreSystem.Models;
using EducationCentreSystem.Common;

namespace EducationCentreSystem.Controllers;

/// <summary>
/// Input model used when updating an existing record.
/// This class demonstrates how to prevent inheritance WITHOUT using the 'sealed' keyword 
/// by using a private constructor and a static factory method (Private Constructor Pattern).
/// </summary>
public class UpdatePersonRequest
{
    public string TargetEmail { get; }
    public string Name { get; }
    public string Telephone { get; }
    public string Subject1 { get; }
    public string Subject2 { get; }
    public string Subject3 { get; }
    public decimal? Salary { get; }
    public string FullTimeOrPartTime { get; }
    public int? WorkingHours { get; }

    /// <summary>
    /// PRIVATE constructor: Prevents any other class from inheriting this class.
    /// </summary>
    private UpdatePersonRequest(
        string targetEmail,
        string name,
        string telephone,
        string subject1,
        string subject2,
        string subject3,
        decimal? salary,
        string fullTimeOrPartTime,
        int? workingHours)
    {
        this.TargetEmail = targetEmail;
        this.Name = name;
        this.Telephone = telephone;
        this.Subject1 = subject1;
        this.Subject2 = subject2;
        this.Subject3 = subject3;
        this.Salary = salary;
        this.FullTimeOrPartTime = fullTimeOrPartTime;
        this.WorkingHours = workingHours;
    }

    /// <summary>
    /// Static factory method: The only way to create an instance of this class from the outside.
    /// </summary>
    public static UpdatePersonRequest Create(
        string targetEmail,
        string name = "",
        string telephone = "",
        string subject1 = "",
        string subject2 = "",
        string subject3 = "",
        decimal? salary = null,
        string fullTimeOrPartTime = "",
        int? workingHours = null)
    {
        return new UpdatePersonRequest(
            targetEmail, name, telephone, 
            subject1, subject2, subject3, 
            salary, fullTimeOrPartTime, workingHours);
    }

    /// <summary>
    /// Validates update input and returns a dictionary of field-to-error messages.
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
