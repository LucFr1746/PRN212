using StudyPlannerDataAccess.Models;

namespace StudyPlannerServices;

public interface ISubjectService
{
    List<Subject> GetAll();
    Subject? GetById(int subjectId);
    void Add(Subject subject);
    void Update(Subject subject);
    void Delete(int subjectId);
}
