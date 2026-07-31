using Microsoft.EntityFrameworkCore;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerDataAccess.DAOs;

public class StudentDAO
{
    private static StudentDAO? _instance;
    private static readonly object _lock = new();

    private StudentDAO() { }

    public static StudentDAO Instance
    {
        get
        {
            lock (_lock)
            {
                _instance ??= new StudentDAO();
                return _instance;
            }
        }
    }

    public Student? GetStudentByCodeAndPassword(string studentCode, string password)
    {
        using var context = new StudyPlannerDbContext();
        return context.Students
            .FirstOrDefault(s => s.StudentCode == studentCode && s.Password == password);
    }

    public Student? GetStudentById(int studentId)
    {
        using var context = new StudyPlannerDbContext();
        return context.Students.Find(studentId);
    }

    public List<Student> GetAllStudents()
    {
        using var context = new StudyPlannerDbContext();
        return context.Students.ToList();
    }
}
