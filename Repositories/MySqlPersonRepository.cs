using EducationCentreSystem.Models;
using EducationCentreSystem.Factories;
using MySqlConnector;

namespace EducationCentreSystem.Repositories;

/// <summary>
/// MySQL-backed repository implementation.
/// Persists data in the People table and maps database rows into the Person inheritance hierarchy.
/// </summary>
/// <remarks>
/// Storage model:
/// - Single table (People) stores common fields (Role, Name, Telephone, Email).
/// - Role-specific fields are stored in nullable columns (StudentSubject*, Teacher*, Admin*).
/// - Role column is used at read time to create the correct derived type and populate only relevant fields.
///
/// Design intent:
/// - UI/controller depend only on IPersonRepository, so switching between MySQL and in-memory requires no changes elsewhere.
/// - All queries are parameterized to avoid SQL injection and handle user input safely.
/// </remarks>
public class MySqlPersonRepository : IPersonRepository
{
    // Centralized column list to avoid repeating the same SELECT projection across queries.
    private const string SelectColumns = "PersonId, Role, Name, Telephone, Email, StudentSubject1, StudentSubject2, StudentSubject3, TeacherSalary, TeacherSubject1, TeacherSubject2, AdminSalary, AdminFullTimeOrPartTime, AdminWorkingHours";

    private string _connectionString;

    /// <summary>
    /// Creates a repository that will connect using the supplied connection string.
    /// </summary>
    public MySqlPersonRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Returns all records from the People table.
    /// </summary>
    public List<Person> GetAll()
    {
        List<Person> list = new List<Person>();
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT " + SelectColumns + " FROM People ORDER BY PersonId;";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Map(reader));
                    }
                }
            }
        }

        return list;
    }

    /// <summary>
    /// Returns all records of a specific role.
    /// </summary>
    public List<Person> GetByRole(PersonRole role)
    {
        List<Person> list = new List<Person>();
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT " + SelectColumns + " FROM People WHERE Role = @Role ORDER BY PersonId;";
                cmd.Parameters.AddWithValue("@Role", role.ToString());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(Map(reader));
                    }
                }
            }
        }

        return list;
    }

    /// <summary>
    /// Finds a record by email (case-insensitive).
    /// Uses a parameterized query to avoid SQL injection.
    /// </summary>
    public Person FindByEmail(string email)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT " + SelectColumns + " FROM People WHERE UPPER(Email) = UPPER(@Email) LIMIT 1;";
                cmd.Parameters.AddWithValue("@Email", email.Trim());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return Map(reader);
                }
            }
        }
    }

    /// <summary>
    /// Checks whether an email already exists in the database.
    /// </summary>
    public bool EmailExists(string email)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1 FROM People WHERE UPPER(Email) = UPPER(@Email) LIMIT 1;";
                cmd.Parameters.AddWithValue("@Email", email.Trim());
                object result = cmd.ExecuteScalar();
                return result != null;
            }
        }
    }

    /// <summary>
    /// Inserts a new record and assigns the auto-generated database Id back to the object.
    /// </summary>
    public Person Add(Person person)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO People (Role, Name, Telephone, Email, StudentSubject1, StudentSubject2, StudentSubject3, TeacherSalary, TeacherSubject1, TeacherSubject2, AdminSalary, AdminFullTimeOrPartTime, AdminWorkingHours) VALUES (@Role, @Name, @Telephone, @Email, @StudentSubject1, @StudentSubject2, @StudentSubject3, @TeacherSalary, @TeacherSubject1, @TeacherSubject2, @AdminSalary, @AdminFullTimeOrPartTime, @AdminWorkingHours); SELECT LAST_INSERT_ID();";

                BindCommon(cmd, person);
                BindRoleSpecific(cmd, person);

                int id = Convert.ToInt32(cmd.ExecuteScalar());
                person.Id = id;
                return person;
            }
        }
    }

    /// <summary>
    /// Updates an existing record by matching on email.
    /// </summary>
    public void Update(Person person)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE People SET Name = @Name, Telephone = @Telephone, StudentSubject1 = @StudentSubject1, StudentSubject2 = @StudentSubject2, StudentSubject3 = @StudentSubject3, TeacherSalary = @TeacherSalary, TeacherSubject1 = @TeacherSubject1, TeacherSubject2 = @TeacherSubject2, AdminSalary = @AdminSalary, AdminFullTimeOrPartTime = @AdminFullTimeOrPartTime, AdminWorkingHours = @AdminWorkingHours WHERE UPPER(Email) = UPPER(@Email);";

                BindCommon(cmd, person);
                BindRoleSpecific(cmd, person);
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Deletes a record by email.
    /// </summary>
    public void DeleteByEmail(string email)
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM People WHERE UPPER(Email) = UPPER(@Email);";
                cmd.Parameters.AddWithValue("@Email", email.Trim());
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Binds parameters common to all roles.
    /// </summary>
    private static void BindCommon(MySqlCommand cmd, Person person)
    {
        // Role is stored as text to simplify readability and mapping from the database.
        cmd.Parameters.AddWithValue("@Role", person.Role.ToString());
        cmd.Parameters.AddWithValue("@Name", person.Name);
        cmd.Parameters.AddWithValue("@Telephone", person.Telephone);
        cmd.Parameters.AddWithValue("@Email", person.Email.Trim());
    }

    /// <summary>
    /// Binds role-specific parameters (Student/Teacher/Admin).
    /// </summary>
    private static void BindRoleSpecific(MySqlCommand cmd, Person person)
    {
        cmd.Parameters.AddWithValue("@StudentSubject1", null);
        cmd.Parameters.AddWithValue("@StudentSubject2", null);
        cmd.Parameters.AddWithValue("@StudentSubject3", null);
        cmd.Parameters.AddWithValue("@TeacherSalary", null);
        cmd.Parameters.AddWithValue("@TeacherSubject1", null);
        cmd.Parameters.AddWithValue("@TeacherSubject2", null);
        cmd.Parameters.AddWithValue("@AdminSalary", null);
        cmd.Parameters.AddWithValue("@AdminFullTimeOrPartTime", null);
        cmd.Parameters.AddWithValue("@AdminWorkingHours", null);

        if (person is Student)
        {
            Student s = (Student)person;
            cmd.Parameters["@StudentSubject1"].Value = s.Subject1;
            cmd.Parameters["@StudentSubject2"].Value = s.Subject2;
            cmd.Parameters["@StudentSubject3"].Value = s.Subject3;
        }

        if (person is Teacher)
        {
            Teacher t = (Teacher)person;
            cmd.Parameters["@TeacherSalary"].Value = t.Salary;
            cmd.Parameters["@TeacherSubject1"].Value = t.Subject1;
            cmd.Parameters["@TeacherSubject2"].Value = t.Subject2;
        }

        if (person is Admin)
        {
            Admin a = (Admin)person;
            cmd.Parameters["@AdminSalary"].Value = a.Salary;
            cmd.Parameters["@AdminFullTimeOrPartTime"].Value = a.FullTimeOrPartTime;
            cmd.Parameters["@AdminWorkingHours"].Value = a.WorkingHours;
        }
    }

    /// <summary>
    /// Maps a database row into the correct derived Person type using the stored Role column.
    /// </summary>
    private static Person Map(MySqlDataReader reader)
    {
        int personId = reader.GetInt32("PersonId");
        string roleText = reader.GetString("Role");
        PersonRole role = (PersonRole)Enum.Parse(typeof(PersonRole), roleText, true);

        Person person = PersonFactory.Create(role);
        person.Id = personId;

        if (person is Student)
        {
            Student s = (Student)person;
            s.Subject1 = GetStringOrEmpty(reader, "StudentSubject1");
            s.Subject2 = GetStringOrEmpty(reader, "StudentSubject2");
            s.Subject3 = GetStringOrEmpty(reader, "StudentSubject3");
        }

        if (person is Teacher)
        {
            Teacher t = (Teacher)person;
            t.Salary = GetDecimalOrZero(reader, "TeacherSalary");
            t.Subject1 = GetStringOrEmpty(reader, "TeacherSubject1");
            t.Subject2 = GetStringOrEmpty(reader, "TeacherSubject2");
        }

        if (person is Admin)
        {
            Admin a = (Admin)person;
            a.Salary = GetDecimalOrZero(reader, "AdminSalary");
            a.FullTimeOrPartTime = GetStringOrEmpty(reader, "AdminFullTimeOrPartTime");
            a.WorkingHours = GetIntOrZero(reader, "AdminWorkingHours");
        }

        person.Name = reader.GetString("Name");
        person.Telephone = reader.GetString("Telephone");
        person.Email = reader.GetString("Email");
        return person;
    }

    /// <summary>
    /// Reads a nullable string column as string.Empty.
    /// </summary>
    private static string GetStringOrEmpty(MySqlDataReader reader, string column)
    {
        int i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? string.Empty : reader.GetString(i);
    }

    /// <summary>
    /// Reads a nullable decimal column as 0m.
    /// </summary>
    private static decimal GetDecimalOrZero(MySqlDataReader reader, string column)
    {
        int i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? 0m : reader.GetDecimal(i);
    }

    /// <summary>
    /// Reads a nullable int column as 0.
    /// </summary>
    private static int GetIntOrZero(MySqlDataReader reader, string column)
    {
        int i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
    }
}
