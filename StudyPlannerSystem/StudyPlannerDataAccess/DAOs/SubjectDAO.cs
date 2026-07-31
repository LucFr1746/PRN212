using Microsoft.EntityFrameworkCore;
using StudyPlannerDataAccess.Models;

namespace StudyPlannerDataAccess.DAOs;

public class SubjectDAO
{
    private static SubjectDAO? _instance;
    private static readonly object _lock = new();

    private SubjectDAO() { }

    public static SubjectDAO Instance
    {
        get
        {
            lock (_lock)
            {
                _instance ??= new SubjectDAO();
                return _instance;
            }
        }
    }

    public List<Subject> GetAllSubjects()
    {
        using var context = new StudyPlannerDbContext();
        return context.Subjects.ToList();
    }

    public Subject? GetSubjectById(int subjectId)
    {
        using var context = new StudyPlannerDbContext();
        return context.Subjects.Find(subjectId);
    }

    public void AddSubject(Subject subject)
    {
        using var context = new StudyPlannerDbContext();
        context.Subjects.Add(subject);
        context.SaveChanges();
    }

    public void UpdateSubject(Subject subject)
    {
        using var context = new StudyPlannerDbContext();
        var existing = context.Subjects.Find(subject.SubjectId);
        if (existing == null)
            throw new InvalidOperationException($"Subject with ID {subject.SubjectId} not found.");

        existing.SubjectCode = subject.SubjectCode;
        existing.SubjectName = subject.SubjectName;
        context.SaveChanges();
    }

    public void DeleteSubject(int subjectId)
    {
        using var context = new StudyPlannerDbContext();
        var subject = context.Subjects
            .Include(s => s.StudyTasks)
            .FirstOrDefault(s => s.SubjectId == subjectId);

        if (subject == null)
            throw new InvalidOperationException($"Subject with ID {subjectId} not found.");

        if (subject.StudyTasks.Any())
            throw new InvalidOperationException("Cannot delete subject that has associated study tasks.");

        context.Subjects.Remove(subject);
        context.SaveChanges();
    }
}
