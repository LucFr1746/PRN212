using StudyPlannerDataAccess.DAOs;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public class StudyTaskRepository : IStudyTaskRepository
{
    public List<StudyTask> GetAll()
        => StudyTaskDAO.Instance.GetAllTasks();

    public List<StudyTask> GetByStudentId(int studentId)
        => StudyTaskDAO.Instance.GetTasksByStudentId(studentId);

    public StudyTask? GetById(int taskId)
        => StudyTaskDAO.Instance.GetTaskById(taskId);

    public void Add(StudyTask task)
        => StudyTaskDAO.Instance.AddTask(task);

    public void Update(StudyTask task)
        => StudyTaskDAO.Instance.UpdateTask(task);

    public void Delete(int taskId)
        => StudyTaskDAO.Instance.DeleteTask(taskId);

    public int GetCompletedCount(int studentId)
        => StudyTaskDAO.Instance.GetCompletedTaskCount(studentId);

    public int GetTotalCount(int studentId)
        => StudyTaskDAO.Instance.GetTotalTaskCount(studentId);
}
