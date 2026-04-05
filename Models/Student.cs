using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models
{
    /// <summary>
    /// Represents a Student entity within the education centre.
    /// This class extends the Person base class and includes student-specific subject enrollments.
    /// </summary>
    public sealed class Student : Person
    {
        /// <summary>
        /// Gets or sets the name of the first subject the student is enrolled in.
        /// </summary>
        public string Subject1 { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the second subject the student is enrolled in.
        /// </summary>
        public string Subject2 { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the third subject the student is enrolled in.
        /// </summary>
        public string Subject3 { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the Student class.
        /// Automatically assigns the Student role via the base constructor.
        /// </summary>
        public Student()
            : base(PersonRole.Student)
        {
        }

        /// <summary>
        /// Extends the base detail string with student-specific subject information.
        /// Demonstrates Polymorphism by overriding the base GetDetails method.
        /// </summary>
        /// <returns>A formatted string containing all student details.</returns>
        public override string GetDetails()
        {
            return base.GetDetails() + " | Subjects: " + Subject1 + ", " + Subject2 + ", " + Subject3;
        }

        /// <summary>
        /// Provides a short string representation of the student, typically used for UI display.
        /// </summary>
        /// <returns>A string formatted as 'Id - Name'.</returns>
        public override string ToString()
        {
            return Id + " - " + Name;
        }

        /// <summary>
        /// Maps student-specific data from a creation request object.
        /// </summary>
        /// <param name="request">The creation request data transfer object.</param>
        public override void MapFromCreateRequest(CreatePersonRequest request)
        {
            if (request != null)
            {
                base.MapFromCreateRequest(request);
                this.Subject1 = request.Subject1;
                this.Subject2 = request.Subject2;
                this.Subject3 = request.Subject3;
            }
        }

        /// <summary>
        /// Updates student-specific data from an update request object.
        /// Only non-blank values are updated.
        /// </summary>
        /// <param name="request">The update request data transfer object.</param>
        public override void MapFromUpdateRequest(UpdatePersonRequest request)
        {
            if (request != null)
            {
                base.MapFromUpdateRequest(request);
                if (string.IsNullOrWhiteSpace(request.Subject1) == false)
                {
                    this.Subject1 = request.Subject1;
                }

                if (string.IsNullOrWhiteSpace(request.Subject2) == false)
                {
                    this.Subject2 = request.Subject2;
                }

                if (string.IsNullOrWhiteSpace(request.Subject3) == false)
                {
                    this.Subject3 = request.Subject3;
                }
            }
        }
    }
}
