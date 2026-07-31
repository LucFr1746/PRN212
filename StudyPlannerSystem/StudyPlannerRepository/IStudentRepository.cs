using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public interface IStudentRepository
{
    Student? Authenticate(string studentCode, string password);
    Student? GetById(int studentId);
    List<Student> GetAll();
}
