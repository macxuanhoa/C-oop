using EducationCentreSystem.Common;
using EducationCentreSystem.Factories;
using EducationCentreSystem.Models;
using EducationCentreSystem.Repositories;
using System;
using System.Collections.Generic;

namespace EducationCentreSystem.Controllers
{
    /// <summary>
    /// Acts as the intermediary between the user interface and the data persistence layer.
    /// This controller orchestrates business logic, input validation, and object mapping.
    /// It demonstrates the use of Dependency Injection via the IPersonRepository interface.
    /// </summary>
    public sealed class PersonController
    {
        /// <summary>
        /// The repository used for persisting and retrieving person records.
        /// </summary>
        private readonly IPersonRepository repository;

        /// <summary>
        /// Initializes a new instance of the PersonController class with a specific repository implementation.
        /// </summary>
        /// <param name="repo">The repository to be used by this controller.</param>
        public PersonController(IPersonRepository repo)
        {
            this.repository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Fetches all people currently stored in the system.
        /// </summary>
        /// <returns>A read-only collection of all Person entities.</returns>
        public IReadOnlyList<Person> ViewAll()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Filters people based on their specific role.
        /// </summary>
        /// <param name="role">The role to filter by.</param>
        /// <returns>A read-only collection of people with the specified role.</returns>
        public IReadOnlyList<Person> ViewByRole(PersonRole role)
        {
            return repository.GetByRole(role);
        }

        /// <summary>
        /// Searches for a person by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The Person if found; otherwise, null.</returns>
        public Person? FindByEmail(string email)
        {
            string? normalizedEmail = ValidationHelper.NormalizeEmail(email);
            if (string.IsNullOrEmpty(normalizedEmail) == true)
            {
                return null;
            }
            return repository.FindByEmail(normalizedEmail);
        }

        /// <summary>
        /// Validates and adds a new person record to the system.
        /// This method demonstrates the use of a Factory pattern for object creation.
        /// </summary>
        /// <param name="request">The data containing the new person's details.</param>
        /// <returns>An OperationResult containing the created Person or error details.</returns>
        public OperationResult<Person> Add(CreatePersonRequest request)
        {
            if (request == null)
            {
                return OperationResult<Person>.Fail("Request data cannot be null.");
            }

            // Step 1: Validate input data using the request's validation logic
            Dictionary<string, string> validationErrors = request.Validate();
            if (validationErrors.Count > 0)
            {
                string combinedErrorMessage = "";
                foreach (var error in validationErrors)
                {
                    combinedErrorMessage = combinedErrorMessage + error.Value + " ";
                }
                return OperationResult<Person>.Fail(combinedErrorMessage.Trim());
            }

            // Step 2: Ensure the email is unique within the system
            string newEmail = ValidationHelper.NormalizeEmail(request.Email)!;
            if (repository.EmailExists(newEmail) == true)
            {
                return OperationResult<Person>.Fail("Validation Error: A record with this email address already exists.");
            }

            // Step 3: Instantiate the correct concrete type using the PersonFactory
            Person newPersonInstance = PersonFactory.Create(request.Role);
            
            // Step 4: Map the request data onto the domain object
            newPersonInstance.MapFromCreateRequest(request);

            // Step 5: Persist the new record via the repository
            repository.Add(newPersonInstance);
            
            return OperationResult<Person>.Ok(newPersonInstance);
        }

        /// <summary>
        /// Validates and updates an existing person record identified by their email.
        /// </summary>
        /// <param name="request">The data containing update details.</param>
        /// <returns>An OperationResult indicating success or containing error messages.</returns>
        public OperationResult Edit(UpdatePersonRequest request)
        {
            if (request == null)
            {
                return OperationResult.Fail("Update request data cannot be null.");
            }

            Dictionary<string, string> errors = request.Validate();
            if (errors.Count > 0)
            {
                string errorSummary = "";
                foreach (string error in errors.Values)
                {
                    errorSummary = errorSummary + error + " ";
                }
                return OperationResult.Fail(errorSummary.Trim());
            }

            string targetEmail = ValidationHelper.NormalizeEmail(request.TargetEmail)!;
            Person? existingPerson = repository.FindByEmail(targetEmail);
            
            if (existingPerson == null)
            {
                return OperationResult.Fail("Update Error: The record you are trying to edit was not found.");
            }

            // Step 1: Apply updates to the domain object
            existingPerson.MapFromUpdateRequest(request);

            // Step 2: Save changes back to the repository
            repository.Update(existingPerson);
            
            return OperationResult.Ok();
        }

        /// <summary>
        /// Removes a person record from the system using their email address as the identifier.
        /// </summary>
        /// <param name="email">The email of the person to be deleted.</param>
        /// <returns>An OperationResult indicating if the deletion was successful.</returns>
        public OperationResult Delete(string email)
        {
            string? normalizedMail = ValidationHelper.NormalizeEmail(email);
            if (string.IsNullOrEmpty(normalizedMail) == true)
            {
                return OperationResult.Fail("Delete Error: Email address is required for identification.");
            }
            
            if (ValidationHelper.IsValidEmail(normalizedMail) == false)
            {
                return OperationResult.Fail("Delete Error: The provided email format is invalid.");
            }

            Person? recordToDelete = repository.FindByEmail(normalizedMail);
            if (recordToDelete == null)
            {
                return OperationResult.Fail("Delete Error: No record found for the specified email.");
            }

            repository.DeleteByEmail(normalizedMail);
            return OperationResult.Ok();
        }
    }
}
