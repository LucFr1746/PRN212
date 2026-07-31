using StudyPlannerDataAccess.Models;
using StudyPlannerRepository;

namespace StudyPlannerBusiness;

public class SubjectBusiness : ISubjectBusiness
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectBusiness()
    {
        _subjectRepository = new SubjectRepository();
    }

    public List<Subject> GetAll()
        => _subjectRepository.GetAll();

    public Subject? GetById(int subjectId)
        => _subjectRepository.GetById(subjectId);

    public void Add(Subject subject)
    {
        if (string.IsNullOrWhiteSpace(subject.SubjectCode))
            throw new ArgumentException("Subject code is required.");

        if (string.IsNullOrWhiteSpace(subject.SubjectName))
            throw new ArgumentException("Subject name is required.");

        _subjectRepository.Add(subject);
    }

    public void Update(Subject subject)
    {
        if (string.IsNullOrWhiteSpace(subject.SubjectCode))
            throw new ArgumentException("Subject code is required.");

        if (string.IsNullOrWhiteSpace(subject.SubjectName))
            throw new ArgumentException("Subject name is required.");

        _subjectRepository.Update(subject);
    }

    public void Delete(int subjectId)
        => _subjectRepository.Delete(subjectId);
}
