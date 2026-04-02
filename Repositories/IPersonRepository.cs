using EducationCentreSystem.Models;

namespace EducationCentreSystem.Repositories;

/// <summary>
/// Persistence contract for storing and retrieving people records.
/// Implementations can store data in different ways (e.g., in-memory collections or a database),
/// while the rest of the application depends only on this interface.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Returns all records currently stored.
    /// </summary>
    IReadOnlyList<Person> GetAll();

    /// <summary>
    /// Returns all records for a specific role.
    /// </summary>
    IReadOnlyList<Person> GetByRole(PersonRole role);

    /// <summary>
    /// Finds a record by email, or returns null if not found.
    /// </summary>
    Person? FindByEmail(string email);

    /// <summary>
    /// Returns true when a record with the given email already exists.
    /// </summary>
    bool EmailExists(string email);

    /// <summary>
    /// Adds a new record and returns the persisted entity (may assign an Id).
    /// </summary>
    Person Add(Person person);

    /// <summary>
    /// Updates an existing record.
    /// </summary>
    void Update(Person person);

    /// <summary>
    /// Deletes a record identified by email.
    /// </summary>
    void DeleteByEmail(string email);
}
