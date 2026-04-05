using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models
{
    /// <summary>
    /// Represents a Teacher entity within the education centre.
    /// This class extends the Person base class and includes teacher-specific data such as salary and subjects taught.
    /// </summary>
    public sealed class Teacher : Person
    {
        /// <summary>
        /// Gets or sets the salary for the teaching staff.
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets the name of the first subject taught by the teacher.
        /// </summary>
        public string Subject1 { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the second subject taught by the teacher.
        /// </summary>
        public string Subject2 { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the Teacher class.
        /// Automatically assigns the Teacher role via the base constructor.
        /// </summary>
        public Teacher()
            : base(PersonRole.Teacher)
        {
        }

        /// <summary>
        /// Extends the base detail string with teacher-specific salary and subject information.
        /// Demonstrates Polymorphism by overriding the base GetDetails method.
        /// </summary>
        /// <returns>A formatted string containing all teacher details.</returns>
        public override string GetDetails()
        {
            return base.GetDetails() + " | Salary: " + Salary + " | Subjects: " + Subject1 + ", " + Subject2;
        }

        /// <summary>
        /// Provides a short string representation of the teacher, typically used for UI display.
        /// </summary>
        /// <returns>A string formatted as 'Id - Name'.</returns>
        public override string ToString()
        {
            return Id + " - " + Name;
        }

        /// <summary>
        /// Maps teacher-specific data from a creation request object.
        /// </summary>
        /// <param name="request">The creation request data transfer object.</param>
        public override void MapFromCreateRequest(CreatePersonRequest request)
        {
            if (request != null)
            {
                base.MapFromCreateRequest(request);
                this.Salary = request.Salary;
                this.Subject1 = request.Subject1;
                this.Subject2 = request.Subject2;
            }
        }

        /// <summary>
        /// Updates teacher-specific data from an update request object.
        /// Only provided values (salary or non-blank subjects) are updated.
        /// </summary>
        /// <param name="request">The update request data transfer object.</param>
        public override void MapFromUpdateRequest(UpdatePersonRequest request)
        {
            if (request != null)
            {
                base.MapFromUpdateRequest(request);
                if (request.Salary.HasValue == true)
                {
                    this.Salary = request.Salary.Value;
                }

                if (string.IsNullOrWhiteSpace(request.Subject1) == false)
                {
                    this.Subject1 = request.Subject1;
                }

                if (string.IsNullOrWhiteSpace(request.Subject2) == false)
                {
                    this.Subject2 = request.Subject2;
                }
            }
        }
    }
}
