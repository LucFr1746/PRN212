using StudyPlannerBusiness;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerServices;

public class StudyTaskService : IStudyTaskService
{
    private readonly IStudyTaskBusiness _studyTaskBusiness;

    public StudyTaskService()
    {
        _studyTaskBusiness = new StudyTaskBusiness();
    }

    public List<StudyTask> GetAll()
        => _studyTaskBusiness.GetAll();

    public List<StudyTask> GetByStudentId(int studentId)
        => _studyTaskBusiness.GetByStudentId(studentId);

    public StudyTask? GetById(int taskId)
        => _studyTaskBusiness.GetById(taskId);

    public void Add(StudyTask task)
        => _studyTaskBusiness.Add(task);

    public void Update(StudyTask task)
        => _studyTaskBusiness.Update(task);

    public void Delete(int taskId)
        => _studyTaskBusiness.Delete(taskId);

    public int GetCompletedCount(int studentId)
        => _studyTaskBusiness.GetCompletedCount(studentId);

    public int GetTotalCount(int studentId)
        => _studyTaskBusiness.GetTotalCount(studentId);

    public double GetCompletionRate(int studentId)
        => _studyTaskBusiness.GetCompletionRate(studentId);
}
