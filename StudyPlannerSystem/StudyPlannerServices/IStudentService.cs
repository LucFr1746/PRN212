using StudyPlannerDataAccess.Models;

namespace StudyPlannerServices;

public interface IStudentService
{
    Student? Authenticate(string studentCode, string password);
    Student? GetById(int studentId);
}
