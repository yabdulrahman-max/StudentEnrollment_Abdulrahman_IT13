using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    /// <summary>
    /// Complete Database Helper for Student Enrollment System
    /// Handles all database operations with SQL Server
    /// </summary>
    public static class DatabaseHelper
    {
        #region Connection String Configuration

        // ===== CONNECTION STRING FOR LocalDB =====
        private static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=EnrollmentDB;Integrated Security=True;";

        // Alternative connection strings (commented out):
        // For SQL Server Express: @"Server=localhost\SQLEXPRESS;Database=EnrollmentDB;Integrated Security=True;";
        // For SQL Server Auth: @"Server=(localdb)\MSSQLLocalDB;Database=EnrollmentDB;User Id=sa;Password=yourpassword;";
        // For Remote Server: @"Server=192.168.1.100;Database=EnrollmentDB;User Id=username;Password=password;";

        #endregion

        #region Core Database Methods

        /// <summary>
        /// Gets a new SQL connection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Tests database connection
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed:\n\n{ex.Message}\n\nPlease check:\n" +
                    "1. SQL Server is running\n" +
                    "2. Connection string is correct\n" +
                    "3. Database exists\n" +
                    "4. You have proper permissions",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Execute SELECT queries and return DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}\n\nError Number: {sqlEx.Number}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing query:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return dt;
        }

        /// <summary>
        /// Execute INSERT, UPDATE, DELETE queries
        /// </summary>
        public static bool ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle specific SQL errors
                switch (sqlEx.Number)
                {
                    case 2627: // Duplicate key
                        MessageBox.Show("This record already exists in the database!",
                            "Duplicate Entry",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        break;
                    case 547: // Foreign key violation
                        MessageBox.Show("Cannot delete this record because it's being used by other records!",
                            "Foreign Key Constraint",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        break;
                    default:
                        MessageBox.Show($"SQL Error: {sqlEx.Message}\n\nError Number: {sqlEx.Number}",
                            "Database Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing command:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Execute scalar queries (COUNT, MAX, SUM, etc.)
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing scalar query:\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
        }

        #endregion

        #region Student Operations

        /// <summary>
        /// Get all active students
        /// </summary>
        public static DataTable GetAllStudents()
        {
            string query = @"SELECT StudentId, FullName, Course, YearLevel, Email, Phone, 
                           CONVERT(VARCHAR, DateRegistered, 23) AS DateRegistered 
                           FROM Students 
                           WHERE IsActive = 1 
                           ORDER BY StudentId DESC";
            return ExecuteQuery(query);
        }

        /// <summary>
        /// Search students by ID, name, or course
        /// </summary>
        public static DataTable SearchStudents(string searchTerm)
        {
            string query = @"SELECT StudentId, FullName, Course, YearLevel, Email, Phone, 
                           CONVERT(VARCHAR, DateRegistered, 23) AS DateRegistered 
                           FROM Students 
                           WHERE IsActive = 1 AND 
                           (StudentId LIKE @search OR 
                            FullName LIKE @search OR 
                            Course LIKE @search OR 
                            Email LIKE @search)
                           ORDER BY StudentId DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@search", "%" + searchTerm + "%")
            };

            return ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Add a new student
        /// </summary>
        public static bool AddStudent(string studentId, string fullName, string course,
            string yearLevel, string email, string phone)
        {
            string query = @"INSERT INTO Students (StudentId, FullName, Course, YearLevel, Email, Phone, DateRegistered) 
                           VALUES (@id, @name, @course, @year, @email, @phone, GETDATE())";

            SqlParameter[] parameters = {
                new SqlParameter("@id", studentId),
                new SqlParameter("@name", fullName),
                new SqlParameter("@course", course),
                new SqlParameter("@year", yearLevel),
                new SqlParameter("@email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email),
                new SqlParameter("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone)
            };

            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Update existing student
        /// </summary>
        public static bool UpdateStudent(string studentId, string fullName, string course,
            string yearLevel, string email, string phone)
        {
            string query = @"UPDATE Students 
                           SET FullName = @name, 
                               Course = @course, 
                               YearLevel = @year, 
                               Email = @email, 
                               Phone = @phone 
                           WHERE StudentId = @id";

            SqlParameter[] parameters = {
                new SqlParameter("@id", studentId),
                new SqlParameter("@name", fullName),
                new SqlParameter("@course", course),
                new SqlParameter("@year", yearLevel),
                new SqlParameter("@email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email),
                new SqlParameter("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone)
            };

            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Delete student (soft delete)
        /// </summary>
        public static bool DeleteStudent(string studentId)
        {
            string query = "UPDATE Students SET IsActive = 0 WHERE StudentId = @id";
            SqlParameter[] parameters = { new SqlParameter("@id", studentId) };
            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Check if student exists
        /// </summary>
        public static bool CheckStudentExists(string studentId)
        {
            string query = "SELECT COUNT(*) FROM Students WHERE StudentId = @id AND IsActive = 1";
            SqlParameter[] parameters = { new SqlParameter("@id", studentId) };
            object result = ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Get student information by ID
        /// </summary>
        public static DataTable GetStudentInfo(string studentId)
        {
            string query = @"SELECT StudentId, FullName, Course, YearLevel, Email, Phone 
                           FROM Students 
                           WHERE StudentId = @id AND IsActive = 1";
            SqlParameter[] parameters = { new SqlParameter("@id", studentId) };
            return ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Generate next student ID
        /// </summary>
        public static string GenerateNextStudentId()
        {
            string query = "SELECT TOP 1 StudentId FROM Students ORDER BY StudentId DESC";
            object result = ExecuteScalar(query);

            if (result != null)
            {
                string lastId = result.ToString();
                // Extract year and number (e.g., "2025-001")
                string[] parts = lastId.Split('-');
                if (parts.Length == 2)
                {
                    string year = parts[0];
                    int number = int.Parse(parts[1]);
                    return $"{year}-{(number + 1):000}";
                }
            }

            return DateTime.Now.Year + "-001";
        }

        #endregion

        #region Subject Operations

        /// <summary>
        /// Get all active subjects
        /// </summary>
        public static DataTable GetAllSubjects()
        {
            string query = @"SELECT SubjectCode, Description, Units, Department, Instructor 
                           FROM Subjects 
                           WHERE IsActive = 1 
                           ORDER BY SubjectCode";
            return ExecuteQuery(query);
        }

        /// <summary>
        /// Search subjects by code, description, or department
        /// </summary>
        public static DataTable SearchSubjects(string searchTerm)
        {
            string query = @"SELECT SubjectCode, Description, Units, Department, Instructor 
                           FROM Subjects 
                           WHERE IsActive = 1 AND 
                           (SubjectCode LIKE @search OR 
                            Description LIKE @search OR 
                            Department LIKE @search OR 
                            Instructor LIKE @search)
                           ORDER BY SubjectCode";

            SqlParameter[] parameters = {
                new SqlParameter("@search", "%" + searchTerm + "%")
            };

            return ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Add a new subject
        /// </summary>
        public static bool AddSubject(string subjectCode, string description, int units,
            string department, string instructor)
        {
            string query = @"INSERT INTO Subjects (SubjectCode, Description, Units, Department, Instructor) 
                           VALUES (@code, @desc, @units, @dept, @instructor)";

            SqlParameter[] parameters = {
                new SqlParameter("@code", subjectCode),
                new SqlParameter("@desc", description),
                new SqlParameter("@units", units),
                new SqlParameter("@dept", department),
                new SqlParameter("@instructor", string.IsNullOrWhiteSpace(instructor) ? (object)DBNull.Value : instructor)
            };

            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Update existing subject
        /// </summary>
        public static bool UpdateSubject(string subjectCode, string description, int units,
            string department, string instructor)
        {
            string query = @"UPDATE Subjects 
                           SET Description = @desc, 
                               Units = @units, 
                               Department = @dept, 
                               Instructor = @instructor 
                           WHERE SubjectCode = @code";

            SqlParameter[] parameters = {
                new SqlParameter("@code", subjectCode),
                new SqlParameter("@desc", description),
                new SqlParameter("@units", units),
                new SqlParameter("@dept", department),
                new SqlParameter("@instructor", string.IsNullOrWhiteSpace(instructor) ? (object)DBNull.Value : instructor)
            };

            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Delete subject (soft delete)
        /// </summary>
        public static bool DeleteSubject(string subjectCode)
        {
            string query = "UPDATE Subjects SET IsActive = 0 WHERE SubjectCode = @code";
            SqlParameter[] parameters = { new SqlParameter("@code", subjectCode) };
            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Check if subject exists
        /// </summary>
        public static bool CheckSubjectExists(string subjectCode)
        {
            string query = "SELECT COUNT(*) FROM Subjects WHERE SubjectCode = @code AND IsActive = 1";
            SqlParameter[] parameters = { new SqlParameter("@code", subjectCode) };
            object result = ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Get subjects for dropdown (formatted)
        /// </summary>
        public static DataTable GetSubjectsForDropdown()
        {
            string query = @"SELECT SubjectCode, 
                           SubjectCode + ' - ' + Description AS DisplayText 
                           FROM Subjects 
                           WHERE IsActive = 1 
                           ORDER BY SubjectCode";
            return ExecuteQuery(query);
        }

        #endregion

        #region Enrollment Operations

        /// <summary>
        /// Get all active enrollments
        /// </summary>
        public static DataTable GetAllEnrollments()
        {
            string query = @"SELECT 
                               e.EnrollmentId, 
                               e.StudentId, 
                               s.FullName AS StudentName, 
                               e.SubjectCode,
                               sub.Description AS SubjectDescription,
                               sub.SubjectCode + ' - ' + sub.Description AS Subject, 
                               e.Semester, 
                               e.YearLevel, 
                               CONVERT(VARCHAR, e.DateEnrolled, 23) AS DateEnrolled,
                               e.Status
                           FROM Enrollments e
                           INNER JOIN Students s ON e.StudentId = s.StudentId
                           INNER JOIN Subjects sub ON e.SubjectCode = sub.SubjectCode
                           WHERE e.Status = 'Active'
                           ORDER BY e.DateEnrolled DESC";

            return ExecuteQuery(query);
        }

        /// <summary>
        /// Get enrollments for a specific student
        /// </summary>
        public static DataTable GetStudentEnrollments(string studentId)
        {
            string query = @"SELECT 
                               e.EnrollmentId, 
                               sub.SubjectCode + ' - ' + sub.Description AS Subject, 
                               e.Semester, 
                               e.YearLevel, 
                               CONVERT(VARCHAR, e.DateEnrolled, 23) AS DateEnrolled
                           FROM Enrollments e
                           INNER JOIN Subjects sub ON e.SubjectCode = sub.SubjectCode
                           WHERE e.StudentId = @studentId AND e.Status = 'Active'
                           ORDER BY e.DateEnrolled DESC";

            SqlParameter[] parameters = { new SqlParameter("@studentId", studentId) };
            return ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Add a new enrollment
        /// </summary>
        public static bool AddEnrollment(string enrollmentId, string studentId, string subjectCode,
            string semester, string yearLevel)
        {
            // Check if student is already enrolled in this subject for the same semester
            if (CheckDuplicateEnrollment(studentId, subjectCode, semester))
            {
                MessageBox.Show("Student is already enrolled in this subject for the selected semester!",
                    "Duplicate Enrollment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            string query = @"INSERT INTO Enrollments (EnrollmentId, StudentId, SubjectCode, Semester, YearLevel, DateEnrolled, Status) 
                           VALUES (@enrollId, @studentId, @subjectCode, @semester, @yearLevel, GETDATE(), 'Active')";

            SqlParameter[] parameters = {
                new SqlParameter("@enrollId", enrollmentId),
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@subjectCode", subjectCode),
                new SqlParameter("@semester", semester),
                new SqlParameter("@yearLevel", yearLevel)
            };

            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Check for duplicate enrollment
        /// </summary>
        private static bool CheckDuplicateEnrollment(string studentId, string subjectCode, string semester)
        {
            string query = @"SELECT COUNT(*) 
                           FROM Enrollments 
                           WHERE StudentId = @studentId 
                           AND SubjectCode = @subjectCode 
                           AND Semester = @semester 
                           AND Status = 'Active'";

            SqlParameter[] parameters = {
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@subjectCode", subjectCode),
                new SqlParameter("@semester", semester)
            };

            object result = ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Cancel enrollment (soft delete)
        /// </summary>
        public static bool CancelEnrollment(string enrollmentId)
        {
            string query = "UPDATE Enrollments SET Status = 'Cancelled' WHERE EnrollmentId = @id";
            SqlParameter[] parameters = { new SqlParameter("@id", enrollmentId) };
            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Generate next enrollment ID
        /// </summary>
        public static string GenerateNextEnrollmentId()
        {
            string query = "SELECT TOP 1 EnrollmentId FROM Enrollments ORDER BY EnrollmentId DESC";
            object result = ExecuteScalar(query);

            if (result != null)
            {
                string lastId = result.ToString();
                int number = int.Parse(lastId.Replace("ENR-", ""));
                return "ENR-" + (number + 1).ToString("000");
            }

            return "ENR-001";
        }

        /// <summary>
        /// Search enrollments by student ID or name
        /// </summary>
        public static DataTable SearchEnrollments(string searchTerm)
        {
            string query = @"SELECT 
                               e.EnrollmentId, 
                               e.StudentId, 
                               s.FullName AS StudentName, 
                               sub.SubjectCode + ' - ' + sub.Description AS Subject, 
                               e.Semester, 
                               e.YearLevel, 
                               CONVERT(VARCHAR, e.DateEnrolled, 23) AS DateEnrolled
                           FROM Enrollments e
                           INNER JOIN Students s ON e.StudentId = s.StudentId
                           INNER JOIN Subjects sub ON e.SubjectCode = sub.SubjectCode
                           WHERE e.Status = 'Active' AND
                           (e.StudentId LIKE @search OR 
                            s.FullName LIKE @search OR
                            e.EnrollmentId LIKE @search)
                           ORDER BY e.DateEnrolled DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@search", "%" + searchTerm + "%")
            };

            return ExecuteQuery(query, parameters);
        }

        #endregion

        #region Dashboard Statistics

        /// <summary>
        /// Get total count of active students
        /// </summary>
        public static int GetStudentCount()
        {
            string query = "SELECT COUNT(*) FROM Students WHERE IsActive = 1";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Get total count of active subjects
        /// </summary>
        public static int GetSubjectCount()
        {
            string query = "SELECT COUNT(*) FROM Subjects WHERE IsActive = 1";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Get total count of active enrollments
        /// </summary>
        public static int GetEnrollmentCount()
        {
            string query = "SELECT COUNT(*) FROM Enrollments WHERE Status = 'Active'";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Get enrollments by semester
        /// </summary>
        public static DataTable GetEnrollmentsBySemester()
        {
            string query = @"SELECT Semester, COUNT(*) AS Count 
                           FROM Enrollments 
                           WHERE Status = 'Active' 
                           GROUP BY Semester 
                           ORDER BY Semester";
            return ExecuteQuery(query);
        }

        /// <summary>
        /// Get students by course
        /// </summary>
        public static DataTable GetStudentsByCourse()
        {
            string query = @"SELECT Course, COUNT(*) AS Count 
                           FROM Students 
                           WHERE IsActive = 1 
                           GROUP BY Course 
                           ORDER BY Count DESC";
            return ExecuteQuery(query);
        }

        /// <summary>
        /// Get recent enrollments (last N days)
        /// </summary>
        public static DataTable GetRecentEnrollments(int days = 7)
        {
            string query = @"SELECT 
                               e.EnrollmentId, 
                               s.FullName AS StudentName, 
                               sub.Description AS Subject,
                               CONVERT(VARCHAR, e.DateEnrolled, 23) AS DateEnrolled
                           FROM Enrollments e
                           INNER JOIN Students s ON e.StudentId = s.StudentId
                           INNER JOIN Subjects sub ON e.SubjectCode = sub.SubjectCode
                           WHERE e.Status = 'Active' 
                           AND e.DateEnrolled >= DATEADD(DAY, -@days, GETDATE())
                           ORDER BY e.DateEnrolled DESC";

            SqlParameter[] parameters = { new SqlParameter("@days", days) };
            return ExecuteQuery(query, parameters);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Backup database to file
        /// </summary>
        public static bool BackupDatabase(string backupPath)
        {
            try
            {
                string query = $"BACKUP DATABASE EnrollmentDB TO DISK = '{backupPath}' WITH FORMAT";
                return ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup failed: {ex.Message}",
                    "Backup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Get database connection info
        /// </summary>
        public static string GetConnectionInfo()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return $"Server: {conn.DataSource}\nDatabase: {conn.Database}\nVersion: {conn.ServerVersion}";
                }
            }
            catch (Exception ex)
            {
                return $"Connection failed: {ex.Message}";
            }
        }

        /// <summary>
        /// Clear all data (for testing purposes)
        /// </summary>
        public static bool ClearAllData()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete ALL data? This action cannot be undone!",
                "Confirm Delete All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = @"
                        DELETE FROM Enrollments;
                        DELETE FROM Subjects;
                        DELETE FROM Students;
                        DBCC CHECKIDENT ('Enrollments', RESEED, 0);
                    ";
                    return ExecuteNonQuery(query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error clearing data: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            return false;
        }

        #endregion
    }
}