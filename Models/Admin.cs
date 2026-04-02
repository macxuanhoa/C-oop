using EducationCentreSystem.Common;
using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models;

/// <summary>
/// Administration staff record: stores salary, job type (full-time/part-time), and working hours.
/// </summary>
public sealed class Admin : Person
{
    /// <summary>
    /// Salary for administration staff.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Job type text ("Full-time" / "Part-time") normalized by validation.
    /// </summary>
    public string FullTimeOrPartTime { get; set; } = string.Empty;

    /// <summary>
    /// Number of working hours for the admin staff member.
    /// </summary>
    public int WorkingHours { get; set; }

    /// <summary>
    /// Constructor fixes the role to Admin by calling the base constructor.
    /// </summary>
    public Admin()
        : base(PersonRole.Admin)
    {
    }

    /// <summary>
    /// Overrides the base display to include admin-specific employment fields (polymorphism).
    /// </summary>
    public override string GetDetails()
    {
        return $"{base.GetDetails()}\n  Salary: {Salary} | Type: {FullTimeOrPartTime} | WorkingHours: {WorkingHours}";
    }

    /// <summary>
    /// Admin specific fields populated from creation request.
    /// </summary>
    public override void MapFromCreateRequest(CreatePersonRequest request)
    {
        base.MapFromCreateRequest(request);
        Salary = request.Salary;
        FullTimeOrPartTime = ValidationHelper.NormalizeJobType(request.FullTimeOrPartTime)!;
        WorkingHours = request.WorkingHours;
    }

    /// <summary>
    /// Admin specific fields updated from update request.
    /// </summary>
    public override void MapFromUpdateRequest(UpdatePersonRequest request)
    {
        base.MapFromUpdateRequest(request);
        if (request.Salary.HasValue) Salary = request.Salary.Value;
        if (!string.IsNullOrWhiteSpace(request.FullTimeOrPartTime))
            FullTimeOrPartTime = ValidationHelper.NormalizeJobType(request.FullTimeOrPartTime)!;
        if (request.WorkingHours.HasValue) WorkingHours = request.WorkingHours.Value;
    }
}
