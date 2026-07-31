using Microsoft.EntityFrameworkCore;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerDataAccess.DAOs;

public class StudyTaskDAO
{
    private static StudyTaskDAO? _instance;
    private static readonly object _lock = new();

    private StudyTaskDAO() { }

    public static StudyTaskDAO Instance
    {
        get
        {
            lock (_lock)
            {
                _instance ??= new StudyTaskDAO();
                return _instance;
            }
        }
    }

    public List<StudyTask> GetAllTasks()
    {
        using var context = new StudyPlannerDbContext();
        return context.StudyTasks
            .Include(t => t.Student)
            .Include(t => t.Subject)
            .ToList();
    }

    public List<StudyTask> GetTasksByStudentId(int studentId)
    {
        using var context = new StudyPlannerDbContext();
        return context.StudyTasks
            .Include(t => t.Subject)
            .Where(t => t.StudentId == studentId)
            .ToList();
    }

    public StudyTask? GetTaskById(int taskId)
    {
        using var context = new StudyPlannerDbContext();
        return context.StudyTasks
            .Include(t => t.Student)
            .Include(t => t.Subject)
            .FirstOrDefault(t => t.TaskId == taskId);
    }

    public void AddTask(StudyTask task)
    {
        using var context = new StudyPlannerDbContext();
        context.StudyTasks.Add(task);
        context.SaveChanges();
    }

    public void UpdateTask(StudyTask task)
    {
        using var context = new StudyPlannerDbContext();
        var existing = context.StudyTasks.Find(task.TaskId);
        if (existing == null)
            throw new InvalidOperationException($"StudyTask with ID {task.TaskId} not found.");

        existing.StudentId = task.StudentId;
        existing.SubjectId = task.SubjectId;
        existing.Description = task.Description;
        existing.DayOfWeek = task.DayOfWeek;
        existing.IsCompleted = task.IsCompleted;
        context.SaveChanges();
    }

    public void DeleteTask(int taskId)
    {
        using var context = new StudyPlannerDbContext();
        var task = context.StudyTasks.Find(taskId);
        if (task == null)
            throw new InvalidOperationException($"StudyTask with ID {taskId} not found.");

        context.StudyTasks.Remove(task);
        context.SaveChanges();
    }

    public int GetCompletedTaskCount(int studentId)
    {
        using var context = new StudyPlannerDbContext();
        return context.StudyTasks.Count(t => t.StudentId == studentId && t.IsCompleted);
    }

    public int GetTotalTaskCount(int studentId)
    {
        using var context = new StudyPlannerDbContext();
        return context.StudyTasks.Count(t => t.StudentId == studentId);
    }
}
