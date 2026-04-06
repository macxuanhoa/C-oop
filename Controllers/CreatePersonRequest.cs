using System.Collections.Generic;
using EducationCentreSystem.Models;
using EducationCentreSystem.Common;

namespace EducationCentreSystem.Controllers;

/// <summary>
/// Input model used when creating a new person record.
/// This class demonstrates how to prevent inheritance WITHOUT using the 'sealed' keyword 
/// by using a private constructor and a static factory method (Private Constructor Pattern).
/// </summary>
public class CreatePersonRequest
{
    public PersonRole Role { get; }
    public string Name { get; }
    public string Telephone { get; }
    public string Email { get; }
    public string Subject1 { get; }
    public string Subject2 { get; }
    public string Subject3 { get; }
    public decimal Salary { get; }
    public string FullTimeOrPartTime { get; }
    public int WorkingHours { get; }

    /// <summary>
    /// PRIVATE constructor: Prevents any other class from inheriting this class,
    /// because a derived class must be able to call its parent's constructor.
    /// </summary>
    private CreatePersonRequest(
        PersonRole role,
        string name,
        string telephone,
        string email,
        string subject1,
        string subject2,
        string subject3,
        decimal salary,
        string fullTimeOrPartTime,
        int workingHours)
    {
        this.Role = role;
        this.Name = name;
        this.Telephone = telephone;
        this.Email = email;
        this.Subject1 = subject1;
        this.Subject2 = subject2;
        this.Subject3 = subject3;
        this.Salary = salary;
        this.FullTimeOrPartTime = fullTimeOrPartTime;
        this.WorkingHours = workingHours;
    }

    /// <summary>
    /// Static factory method: The only way to create an instance of this class from the outside.
    /// This provides a controlled entry point for object creation.
    /// </summary>
    public static CreatePersonRequest Create(
        PersonRole role,
        string name,
        string telephone,
        string email,
        string subject1 = "",
        string subject2 = "",
        string subject3 = "",
        decimal salary = 0,
        string fullTimeOrPartTime = "",
        int workingHours = 0)
    {
        return new CreatePersonRequest(
            role, name, telephone, email, 
            subject1, subject2, subject3, 
            salary, fullTimeOrPartTime, workingHours);
    }

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
