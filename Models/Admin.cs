namespace EducationCentreSystem.Models
{
    using EducationCentreSystem.Common;
    using EducationCentreSystem.Controllers;

    /// <summary>
    /// Represents an Administrative staff entity within the education centre.
    /// This class extends the Person base class and includes administrative-specific data such as salary, employment type, and working hours.
    /// </summary>
    public sealed class Admin : Person
    {
        /// <summary>
        /// Gets or sets the salary for the administrative staff.
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets the employment type (e.g., "Full-time" or "Part-time").
        /// The value is normalized via the ValidationHelper.
        /// </summary>
        public string FullTimeOrPartTime { get; set; } = "";

        /// <summary>
        /// Gets or sets the total weekly working hours for the staff member.
        /// </summary>
        public int WorkingHours { get; set; }

        /// <summary>
        /// Initializes a new instance of the Admin class.
        /// Automatically assigns the Admin role via the base constructor.
        /// </summary>
        public Admin()
            : base(PersonRole.Admin)
        {
        }

        /// <summary>
        /// Extends the base detail string with administrative-specific employment information.
        /// Demonstrates Polymorphism by overriding the base GetDetails method.
        /// </summary>
        /// <returns>A formatted string containing all administrative details.</returns>
        public override string GetDetails()
        {
            return base.GetDetails() + "\n  Salary: " + Salary + " | Type: " + FullTimeOrPartTime + " | WorkingHours: " + WorkingHours;
        }

        /// <summary>
        /// Maps administrative-specific data from a creation request object.
        /// </summary>
        /// <param name="request">The creation request data transfer object.</param>
        public override void MapFromCreateRequest(CreatePersonRequest request)
        {
            base.MapFromCreateRequest(request);
            this.Salary = request.Salary;
            this.FullTimeOrPartTime = ValidationHelper.NormalizeJobType(request.FullTimeOrPartTime)!;
            this.WorkingHours = request.WorkingHours;
        }

        /// <summary>
        /// Updates administrative-specific data from an update request object.
        /// Only provided values (salary, non-blank job type, or hours) are updated.
        /// </summary>
        /// <param name="request">The update request data transfer object.</param>
        public override void MapFromUpdateRequest(UpdatePersonRequest request)
        {
            base.MapFromUpdateRequest(request);
            if (request.Salary.HasValue == true)
            {
                this.Salary = request.Salary.Value;
            }

            if (string.IsNullOrWhiteSpace(request.FullTimeOrPartTime) == false)
            {
                this.FullTimeOrPartTime = ValidationHelper.NormalizeJobType(request.FullTimeOrPartTime)!;
            }

            if (request.WorkingHours.HasValue == true)
            {
                this.WorkingHours = request.WorkingHours.Value;
            }
        }
    }
}
