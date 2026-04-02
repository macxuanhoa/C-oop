using EducationCentreSystem.Common;
using EducationCentreSystem.Factories;
using EducationCentreSystem.Models;
using EducationCentreSystem.Repositories;

namespace EducationCentreSystem.Controllers;

/// <summary>
/// Coordinates user actions (add/view/edit/delete) and enforces basic business rules.
/// The controller depends on the repository abstraction (IPersonRepository) so that storage can be swapped
/// (e.g., in-memory vs MySQL) without changing the UI or controller logic.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Validate incoming requests (delegated to request models)
/// - Normalize common input (email/telephone/job type)
/// - Enforce unique email constraint
/// - Create the correct derived Person type using the factory
/// - Delegate persistence to the repository
///
/// This separation keeps the UI focused on input/output while the controller handles application rules.
/// </remarks>
public sealed class PersonController
{
    private readonly IPersonRepository _repo;

    /// <summary>
    /// Creates a controller that operates on the provided repository.
    /// </summary>
    public PersonController(IPersonRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Returns all stored people (students, teachers, and admins).
    /// </summary>
    public IReadOnlyList<Person> ViewAll()
    {
        return _repo.GetAll();
    }

    /// <summary>
    /// Returns people filtered by a specific role.
    /// </summary>
    public IReadOnlyList<Person> ViewByRole(PersonRole role)
    {
        return _repo.GetByRole(role);
    }

    /// <summary>
    /// Finds a person by email. Email is normalized before lookup.
    /// Returns null when the email is blank/invalid or no record exists.
    /// </summary>
    public Person? FindByEmail(string email)
    {
        var normalized = ValidationHelper.NormalizeEmail(email);
        if (normalized == null) return null;
        return _repo.FindByEmail(normalized);
    }

    /// <summary>
    /// Adds a new record after validating input and enforcing unique email.
    /// Demonstrates polymorphism: a Person variable can reference Student/Teacher/Admin created by the factory,
    /// while role-specific fields are populated via type checks.
    /// </summary>
    public OperationResult<Person> Add(CreatePersonRequest request)
    {
        var validationErrors = request.Validate();
        if (validationErrors.Count > 0)
        {
            return OperationResult<Person>.Fail(string.Join(" ", validationErrors.Values));
        }

        // Email is guaranteed to be valid and not null by request.Validate()
        var email = ValidationHelper.NormalizeEmail(request.Email)!;

        if (_repo.EmailExists(email)) return OperationResult<Person>.Fail("Email already exists.");

        // Factory method returns the correct derived type based on role.
        var person = PersonFactory.Create(request.Role);
        
        // Use polymorphism to populate the object based on its actual type.
        person.MapFromCreateRequest(request);

        // Repository implementation decides how persistence happens (in-memory list vs database table).
        _repo.Add(person);
        return OperationResult<Person>.Ok(person);
    }

    /// <summary>
    /// Updates an existing record identified by TargetEmail.
    /// Only fields provided in the request are updated; blank values keep the current data.
    /// </summary>
    public OperationResult Edit(UpdatePersonRequest request)
    {
        var validationErrors = request.Validate();
        if (validationErrors.Count > 0)
        {
            return OperationResult.Fail(string.Join(" ", validationErrors.Values));
        }

        // TargetEmail is guaranteed to be valid by request.Validate()
        var targetEmail = ValidationHelper.NormalizeEmail(request.TargetEmail)!;

        var person = _repo.FindByEmail(targetEmail);
        if (person == null) return OperationResult.Fail("Record not found.");

        // Use polymorphism to update the object based on its actual type.
        person.MapFromUpdateRequest(request);

        _repo.Update(person);
        return OperationResult.Ok();
    }

    /// <summary>
    /// Deletes a record by email. Returns failure when the email is missing/invalid or the record does not exist.
    /// </summary>
    public OperationResult Delete(string email)
    {
        var normalized = ValidationHelper.NormalizeEmail(email);
        if (normalized == null) return OperationResult.Fail("Email is required.");
        if (!ValidationHelper.IsValidEmail(normalized)) return OperationResult.Fail("Email format is invalid.");

        var existing = _repo.FindByEmail(normalized);
        if (existing == null) return OperationResult.Fail("Record not found.");
        _repo.DeleteByEmail(normalized);
        return OperationResult.Ok();
    }
}
