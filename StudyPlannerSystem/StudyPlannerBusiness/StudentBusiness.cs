using StudyPlannerDataAccess.Models;
using StudyPlannerRepository;

namespace StudyPlannerBusiness;

public class StudentBusiness : IStudentBusiness
{
    private readonly IStudentRepository _studentRepository;

    public StudentBusiness()
    {
        _studentRepository = new StudentRepository();
    }

    public Student? Authenticate(string studentCode, string password)
    {
        if (string.IsNullOrWhiteSpace(studentCode) || string.IsNullOrWhiteSpace(password))
            return null;

        return _studentRepository.Authenticate(studentCode, password);
    }

    public Student? GetById(int studentId)
        => _studentRepository.GetById(studentId);
}
