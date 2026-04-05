namespace EducationCentreSystem.Factories
{
    using EducationCentreSystem.Models;
    using System;

    // This class is used to create Student, Teacher or Admin objects
    // based on the provided Role.
    public static class PersonFactory
    {
        public static Person Create(PersonRole role)
        {
            // Use traditional switch case to control object creation manually
            switch (role)
            {
                case PersonRole.Student:
                    Student newStudent = new Student();
                    return newStudent;

                case PersonRole.Teacher:
                    Teacher newTeacher = new Teacher();
                    return newTeacher;

                case PersonRole.Admin:
                    Admin newAdmin = new Admin();
                    return newAdmin;

                default:
                    // Throw an exception if the role is not recognized
                    throw new Exception("Could not find a matching Person type for role: " + role);
            }
        }
    }
}
