using StudyPlannerDataAccess.Models;

namespace StudyPlannerBusiness;

public interface ISubjectBusiness
{
    List<Subject> GetAll();
    Subject? GetById(int subjectId);
    void Add(Subject subject);
    void Update(Subject subject);
    void Delete(int subjectId);
}
