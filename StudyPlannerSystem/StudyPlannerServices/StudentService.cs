using StudyPlannerBusiness;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerServices;

public class StudentService : IStudentService
{
    private readonly IStudentBusiness _studentBusiness;

    public StudentService()
    {
        _studentBusiness = new StudentBusiness();
    }

    public Student? Authenticate(string studentCode, string password)
        => _studentBusiness.Authenticate(studentCode, password);

    public Student? GetById(int studentId)
        => _studentBusiness.GetById(studentId);
}
