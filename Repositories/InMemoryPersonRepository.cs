using EducationCentreSystem.Models;
using EducationCentreSystem.Common;
using System;
using System.Collections.Generic;

namespace EducationCentreSystem.Repositories
{
    /// <summary>
    /// Implementation of IPersonRepository that stores data in system memory.
    /// This is ideal for development and testing as it does not require an external database.
    /// Data is stored in a private List and is cleared when the application terminates.
    /// </summary>
    public class InMemoryPersonRepository : IPersonRepository
    {
        /// <summary>
        /// Internal collection of all people records (Students, Teachers, and Admins).
        /// </summary>
        private List<Person> peopleList = new List<Person>();

        /// <summary>
        /// Tracks the next available ID to ensure unique primary keys for new records.
        /// </summary>
        private int nextIdCounter = 1;

        /// <summary>
        /// Retrieves a copy of all person records stored in memory.
        /// </summary>
        /// <returns>A read-only list containing all people.</returns>
        public List<Person> GetAll()
        {
            List<Person> snapshot = new List<Person>();
            foreach (Person p in peopleList)
            {
                snapshot.Add(p);
            }
            return snapshot;
        }

        /// <summary>
        /// Filters and retrieves people based on their specific role.
        /// </summary>
        /// <param name="role">The role to filter by (e.g., Student, Teacher, Admin).</param>
        /// <returns>A read-only list of people matching the specified role.</returns>
        public List<Person> GetByRole(PersonRole role)
        {
            List<Person> filteredResults = new List<Person>();
            for (int i = 0; i < peopleList.Count; i++)
            {
                Person p = peopleList[i];
                if (p.Role == role)
                {
                    filteredResults.Add(p);
                }
            }
            return filteredResults;
        }

        /// <summary>
        /// Searches for a specific person using their email address.
        /// Performs a case-insensitive search to ensure reliability.
        /// </summary>
        /// <param name="email">The email address to look for.</param>
        /// <returns>The Person object if found; otherwise, null.</returns>
        public Person? FindByEmail(string email)
        {
            string? normalizedTarget = ValidationHelper.NormalizeEmail(email);
            if (normalizedTarget == null)
            {
                return null;
            }

            foreach (Person item in peopleList)
            {
                string? currentEmail = ValidationHelper.NormalizeEmail(item.Email);
                if (string.Equals(currentEmail, normalizedTarget, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Determines whether a person with the given email address already exists in the store.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is taken; false otherwise.</returns>
        public bool EmailExists(string email)
        {
            Person? existingRecord = FindByEmail(email);
            return (existingRecord != null);
        }

        /// <summary>
        /// Persists a new person record to the in-memory store.
        /// Automatically assigns a unique sequential ID if one is not already provided.
        /// </summary>
        /// <param name="person">The person record to add.</param>
        /// <returns>The added person record with its assigned ID.</returns>
        public Person Add(Person person)
        {
            if (person != null)
            {
                if (person.Id <= 0)
                {
                    person.Id = nextIdCounter;
                    nextIdCounter = nextIdCounter + 1;
                }
                peopleList.Add(person);
            }
            return person!;
        }

        /// <summary>
        /// Updates an existing person record by matching on their email address.
        /// If found, the old record is replaced with the new one.
        /// </summary>
        /// <param name="person">The updated person record.</param>
        public void Update(Person person)
        {
            if (person != null)
            {
                Person? oldRecord = FindByEmail(person.Email);
                if (oldRecord != null)
                {
                    peopleList.Remove(oldRecord);
                    peopleList.Add(person);
                }
            }
        }

        /// <summary>
        /// Permanently removes a person record from the store based on their email address.
        /// </summary>
        /// <param name="email">The email of the person to delete.</param>
        public void DeleteByEmail(string email)
        {
            Person? targetToRemove = FindByEmail(email);
            if (targetToRemove != null)
            {
                peopleList.Remove(targetToRemove);
            }
        }
    }
}
