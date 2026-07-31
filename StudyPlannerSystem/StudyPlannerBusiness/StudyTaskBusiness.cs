using StudyPlannerDataAccess.Models;
using StudyPlannerRepository;

namespace StudyPlannerBusiness;

public class StudyTaskBusiness : IStudyTaskBusiness
{
    private readonly IStudyTaskRepository _studyTaskRepository;

    public StudyTaskBusiness()
    {
        _studyTaskRepository = new StudyTaskRepository();
    }

    public List<StudyTask> GetAll()
        => _studyTaskRepository.GetAll();

    public List<StudyTask> GetByStudentId(int studentId)
        => _studyTaskRepository.GetByStudentId(studentId);

    public StudyTask? GetById(int taskId)
        => _studyTaskRepository.GetById(taskId);

    public void Add(StudyTask task)
    {
        if (string.IsNullOrWhiteSpace(task.Description))
            throw new ArgumentException("Task description is required.");

        _studyTaskRepository.Add(task);
    }

    public void Update(StudyTask task)
        => _studyTaskRepository.Update(task);

    public void Delete(int taskId)
        => _studyTaskRepository.Delete(taskId);

    public int GetCompletedCount(int studentId)
        => _studyTaskRepository.GetCompletedCount(studentId);

    public int GetTotalCount(int studentId)
        => _studyTaskRepository.GetTotalCount(studentId);

    public double GetCompletionRate(int studentId)
    {
        int total = _studyTaskRepository.GetTotalCount(studentId);
        if (total == 0)
            return 0;

        int completed = _studyTaskRepository.GetCompletedCount(studentId);
        return (double)completed / total * 100;
    }
}
