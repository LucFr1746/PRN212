using StudyPlannerDataAccess.DAOs;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public class StudentRepository : IStudentRepository
{
    public Student? Authenticate(string studentCode, string password)
        => StudentDAO.Instance.GetStudentByCodeAndPassword(studentCode, password);

    public Student? GetById(int studentId)
        => StudentDAO.Instance.GetStudentById(studentId);

    public List<Student> GetAll()
        => StudentDAO.Instance.GetAllStudents();
}
