using StudyPlannerDataAccess.Models;

namespace StudyPlannerRepository;

public interface IStudyTaskRepository
{
    List<StudyTask> GetAll();
    List<StudyTask> GetByStudentId(int studentId);
    StudyTask? GetById(int taskId);
    void Add(StudyTask task);
    void Update(StudyTask task);
    void Delete(int taskId);
    int GetCompletedCount(int studentId);
    int GetTotalCount(int studentId);
}
