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
public sealed class MySqlPersonRepository : IPersonRepository
{
    // Centralized column list to avoid repeating the same SELECT projection across queries.
    private const string SelectColumns = 
        """
        PersonId, Role, Name, Telephone, Email,
        StudentSubject1, StudentSubject2, StudentSubject3,
        TeacherSalary, TeacherSubject1, TeacherSubject2,
        AdminSalary, AdminFullTimeOrPartTime, AdminWorkingHours
        """;

    private readonly string _connectionString;

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
    public IReadOnlyList<Person> GetAll()
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            $"""
            -- Read all records in a stable order (by PersonId).
            -- The projection matches the People table schema and supports mapping into derived types.
            SELECT {SelectColumns}
            FROM People
            ORDER BY PersonId;
            """;

        using var reader = cmd.ExecuteReader();
        var list = new List<Person>();
        while (reader.Read())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    /// <summary>
    /// Returns all records of a specific role.
    /// </summary>
    public IReadOnlyList<Person> GetByRole(PersonRole role)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            $"""
            -- Filter by role (Teacher/Admin/Student) while using the same projection as GetAll.
            SELECT {SelectColumns}
            FROM People
            WHERE Role = @Role
            ORDER BY PersonId;
            """;
        cmd.Parameters.AddWithValue("@Role", role.ToString());

        using var reader = cmd.ExecuteReader();
        var list = new List<Person>();
        while (reader.Read())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    /// <summary>
    /// Finds a record by email (case-insensitive).
    /// Uses a parameterized query to avoid SQL injection.
    /// </summary>
    public Person? FindByEmail(string email)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            $"""
            -- Email is treated as case-insensitive for lookup to match UI/controller behavior.
            SELECT {SelectColumns}
            FROM People
            WHERE UPPER(Email) = UPPER(@Email)
            LIMIT 1;
            """;
        cmd.Parameters.AddWithValue("@Email", email.Trim());

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return Map(reader);
    }

    /// <summary>
    /// Checks whether an email already exists in the database.
    /// </summary>
    public bool EmailExists(string email)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        // Uses a lightweight scalar query to avoid fetching a full row when only existence is needed.
        cmd.CommandText = "SELECT 1 FROM People WHERE UPPER(Email) = UPPER(@Email) LIMIT 1;";
        cmd.Parameters.AddWithValue("@Email", email.Trim());
        var result = cmd.ExecuteScalar();
        return result != null;
    }

    /// <summary>
    /// Inserts a new record and assigns the auto-generated database Id back to the object.
    /// </summary>
    public Person Add(Person person)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
            -- Insert stores both common fields and role-specific nullable columns in one table.
            INSERT INTO People
                (Role, Name, Telephone, Email,
                 StudentSubject1, StudentSubject2, StudentSubject3,
                 TeacherSalary, TeacherSubject1, TeacherSubject2,
                 AdminSalary, AdminFullTimeOrPartTime, AdminWorkingHours)
            VALUES
                (@Role, @Name, @Telephone, @Email,
                 @StudentSubject1, @StudentSubject2, @StudentSubject3,
                 @TeacherSalary, @TeacherSubject1, @TeacherSubject2,
                 @AdminSalary, @AdminFullTimeOrPartTime, @AdminWorkingHours);
            SELECT LAST_INSERT_ID();
            """;

        // Bind parameters as values (prevents SQL injection and handles quoting correctly).
        BindCommon(cmd, person);
        BindRoleSpecific(cmd, person);

        // MySQL returns the generated identity value; assign it back to the in-memory object.
        var id = Convert.ToInt32(cmd.ExecuteScalar());
        person.Id = id;
        return person;
    }

    /// <summary>
    /// Updates an existing record by matching on email.
    /// </summary>
    public void Update(Person person)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
            -- Update matches by email, because email is treated as the stable identifier in this project.
            -- Only role-specific columns relevant to the runtime type should be meaningful; others remain null/empty.
            UPDATE People
            SET Name = @Name,
                Telephone = @Telephone,
                StudentSubject1 = @StudentSubject1,
                StudentSubject2 = @StudentSubject2,
                StudentSubject3 = @StudentSubject3,
                TeacherSalary = @TeacherSalary,
                TeacherSubject1 = @TeacherSubject1,
                TeacherSubject2 = @TeacherSubject2,
                AdminSalary = @AdminSalary,
                AdminFullTimeOrPartTime = @AdminFullTimeOrPartTime,
                AdminWorkingHours = @AdminWorkingHours
            WHERE UPPER(Email) = UPPER(@Email);
            """;

        // Same binding helpers are reused to keep insert/update consistent.
        BindCommon(cmd, person);
        BindRoleSpecific(cmd, person);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Deletes a record by email.
    /// </summary>
    public void DeleteByEmail(string email)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        // Parameterized delete avoids string concatenation and is safe for user-provided input.
        cmd.CommandText = "DELETE FROM People WHERE UPPER(Email) = UPPER(@Email);";
        cmd.Parameters.AddWithValue("@Email", email.Trim());
        cmd.ExecuteNonQuery();
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
        // Student columns are null for non-students.
        cmd.Parameters.AddWithValue("@StudentSubject1", person is Student s ? s.Subject1 : null);
        cmd.Parameters.AddWithValue("@StudentSubject2", person is Student s2 ? s2.Subject2 : null);
        cmd.Parameters.AddWithValue("@StudentSubject3", person is Student s3 ? s3.Subject3 : null);

        // Teacher columns are null for non-teachers.
        cmd.Parameters.AddWithValue("@TeacherSalary", person is Teacher t ? t.Salary : null);
        cmd.Parameters.AddWithValue("@TeacherSubject1", person is Teacher t1 ? t1.Subject1 : null);
        cmd.Parameters.AddWithValue("@TeacherSubject2", person is Teacher t2 ? t2.Subject2 : null);

        // Admin columns are null for non-admins.
        cmd.Parameters.AddWithValue("@AdminSalary", person is Admin a ? a.Salary : null);
        cmd.Parameters.AddWithValue("@AdminFullTimeOrPartTime", person is Admin a1 ? a1.FullTimeOrPartTime : null);
        cmd.Parameters.AddWithValue("@AdminWorkingHours", person is Admin a2 ? a2.WorkingHours : null);
    }

    /// <summary>
    /// Maps a database row into the correct derived Person type using the stored Role column.
    /// </summary>
    private static Person Map(MySqlDataReader reader)
    {
        // Row contains a Role column; use it to construct the correct derived type.
        var personId = reader.GetInt32("PersonId");
        var roleText = reader.GetString("Role");
        var role = Enum.Parse<PersonRole>(roleText, ignoreCase: true);

        // Factory chooses Student/Teacher/Admin so the returned object matches the role.
        var person = PersonFactory.Create(role);
        person.Id = personId;

        if (person is Student s)
        {
            // Students store 3 subjects.
            s.Subject1 = GetStringOrEmpty(reader, "StudentSubject1");
            s.Subject2 = GetStringOrEmpty(reader, "StudentSubject2");
            s.Subject3 = GetStringOrEmpty(reader, "StudentSubject3");
        }

        if (person is Teacher t)
        {
            // Teachers store salary and 2 subjects.
            t.Salary = GetDecimalOrZero(reader, "TeacherSalary");
            t.Subject1 = GetStringOrEmpty(reader, "TeacherSubject1");
            t.Subject2 = GetStringOrEmpty(reader, "TeacherSubject2");
        }

        if (person is Admin a)
        {
            // Admins store salary, job type, and working hours.
            a.Salary = GetDecimalOrZero(reader, "AdminSalary");
            a.FullTimeOrPartTime = GetStringOrEmpty(reader, "AdminFullTimeOrPartTime");
            a.WorkingHours = GetIntOrZero(reader, "AdminWorkingHours");
        }

        // Common fields are populated for all roles.
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
        var i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? string.Empty : reader.GetString(i);
    }

    /// <summary>
    /// Reads a nullable decimal column as 0m.
    /// </summary>
    private static decimal GetDecimalOrZero(MySqlDataReader reader, string column)
    {
        var i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? 0m : reader.GetDecimal(i);
    }

    /// <summary>
    /// Reads a nullable int column as 0.
    /// </summary>
    private static int GetIntOrZero(MySqlDataReader reader, string column)
    {
        var i = reader.GetOrdinal(column);
        return reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
    }
}
