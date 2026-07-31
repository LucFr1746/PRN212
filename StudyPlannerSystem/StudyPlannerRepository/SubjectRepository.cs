using StudyPlannerDataAccess.DAOs;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public class SubjectRepository : ISubjectRepository
{
    public List<Subject> GetAll()
        => SubjectDAO.Instance.GetAllSubjects();

    public Subject? GetById(int subjectId)
        => SubjectDAO.Instance.GetSubjectById(subjectId);

    public void Add(Subject subject)
        => SubjectDAO.Instance.AddSubject(subject);

    public void Update(Subject subject)
        => SubjectDAO.Instance.UpdateSubject(subject);

    public void Delete(int subjectId)
        => SubjectDAO.Instance.DeleteSubject(subjectId);
}
