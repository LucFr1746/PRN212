using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public interface ISubjectRepository
{
    List<Subject> GetAll();
    Subject? GetById(int subjectId);
    void Add(Subject subject);
    void Update(Subject subject);
    void Delete(int subjectId);
}
