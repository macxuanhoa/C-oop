using EducationCentreSystem.Common;
using EducationCentreSystem.Controllers;

namespace EducationCentreSystem.Models
{
    /// <summary>
    /// Represents the base entity for all individuals within the education centre.
    /// This class is abstract to enforce that only specific roles (Student, Teacher, Admin) can be instantiated.
    /// It demonstrates key OOP principles: Abstraction, Encapsulation, and Polymorphism.
    /// </summary>
    public abstract class Person
    {
        /// <summary>
        /// Gets or sets the unique identifier for the record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the telephone contact number.
        /// </summary>
        public string Telephone { get; set; } = "";

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// Gets the assigned role for this person. The role is read-only after creation to maintain data integrity.
        /// </summary>
        public PersonRole Role { get; }

        /// <summary>
        /// Initializes a new instance of the Person class with a specific role.
        /// Protected constructor to be called by derived classes only.
        /// </summary>
        /// <param name="role">The role assigned to the person.</param>
        protected Person(PersonRole role)
        {
            this.Role = role;
        }

        /// <summary>
        /// Generates a formatted string representing the basic details of the person.
        /// This method is virtual to allow role-specific data to be appended by subclasses (Polymorphism).
        /// </summary>
        /// <returns>A string containing the common person details.</returns>
        public virtual string GetDetails()
        {
            return "ID: " + Id + " | Role: " + Role + " | Name: " + Name + " | Telephone: " + Telephone + " | Email: " + Email;
        }

        /// <summary>
        /// Populates common fields from a creation request object.
        /// This method ensures that input data is properly trimmed and normalized.
        /// </summary>
        /// <param name="request">The data transfer object containing creation details.</param>
        
        public virtual void MapFromCreateRequest(CreatePersonRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            this.Name = request.Name.Trim();
            this.Email = ValidationHelper.NormalizeEmail(request.Email)!;
            this.Telephone = ValidationHelper.NormalizeTelephone(request.Telephone)!;
        }

        /// <summary>
        /// Updates existing common fields from an update request object.
        /// Only non-blank values are applied to prevent overwriting existing data with empty strings.
        /// </summary>
        /// <param name="request">The data transfer object containing update details.</param>
        public virtual void MapFromUpdateRequest(UpdatePersonRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            if (string.IsNullOrWhiteSpace(request.Name) == false)
            {
                this.Name = request.Name.Trim();
            }

            if (string.IsNullOrWhiteSpace(request.Telephone) == false)
            {
                this.Telephone = ValidationHelper.NormalizeTelephone(request.Telephone)!;
            }
        }
    }
}
