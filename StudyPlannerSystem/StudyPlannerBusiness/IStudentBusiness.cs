using StudyPlannerDataAccess.Models;

namespace StudyPlannerBusiness;

public interface IStudentBusiness
{
    Student? Authenticate(string studentCode, string password);
    Student? GetById(int studentId);
}
