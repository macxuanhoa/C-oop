using EducationCentreSystem.Models;
using EducationCentreSystem.Common;

namespace EducationCentreSystem.Repositories;

/// <summary>
/// In-memory repository implementation.
/// Stores records in a List{Person} so the application can run without a database.
/// Data is not persisted between application runs.
/// </summary>
public sealed class InMemoryPersonRepository : IPersonRepository
{
    // Internal storage for an unbounded number of records (grows as needed).
    private readonly List<Person> _people = new();

    // Simple Id generator used when a new record is added without an Id.
    private int _nextId = 1;

    /// <summary>
    /// Returns a snapshot copy of all records.
    /// </summary>
    public IReadOnlyList<Person> GetAll()
    {
        return _people.ToList();
    }

    /// <summary>
    /// Returns a snapshot copy filtered by role.
    /// </summary>
    public IReadOnlyList<Person> GetByRole(PersonRole role)
    {
        return _people.Where(p => p.Role == role).ToList();
    }

    /// <summary>
    /// Finds a record by email (case-insensitive).
    /// </summary>
    public Person? FindByEmail(string email)
    {
        var target = ValidationHelper.NormalizeEmail(email);
        if (target == null) return null;
        return _people.FirstOrDefault(p => 
            string.Equals(ValidationHelper.NormalizeEmail(p.Email), target, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Checks whether the given email already exists in the store.
    /// </summary>
    public bool EmailExists(string email)
    {
        return FindByEmail(email) != null;
    }

    /// <summary>
    /// Adds a new record to the in-memory list. Assigns a sequential Id when needed.
    /// </summary>
    public Person Add(Person person)
    {
        if (person.Id <= 0) person.Id = _nextId++;
        _people.Add(person);
        return person;
    }

    /// <summary>
    /// Updates an existing record by matching on email.
    /// </summary>
    public void Update(Person person)
    {
        var existing = FindByEmail(person.Email);
        if (existing == null) return;
        _people.Remove(existing);
        _people.Add(person);
    }

    /// <summary>
    /// Deletes an existing record by email.
    /// </summary>
    public void DeleteByEmail(string email)
    {
        var existing = FindByEmail(email);
        if (existing != null) _people.Remove(existing);
    }
}
