using StudyPlannerBusiness;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerServices;

public class SubjectService : ISubjectService
{
    private readonly ISubjectBusiness _subjectBusiness;

    public SubjectService()
    {
        _subjectBusiness = new SubjectBusiness();
    }

    public List<Subject> GetAll()
        => _subjectBusiness.GetAll();

    public Subject? GetById(int subjectId)
        => _subjectBusiness.GetById(subjectId);

    public void Add(Subject subject)
        => _subjectBusiness.Add(subject);

    public void Update(Subject subject)
        => _subjectBusiness.Update(subject);

    public void Delete(int subjectId)
        => _subjectBusiness.Delete(subjectId);
}
