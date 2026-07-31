# Reusable LINQ Query Patterns

This library contains query patterns frequently required during the PRN212 practical exam. Use these directly in your services or repositories to load data and build dynamic filter panels.

---

## 1. Eager Loading (Nested Relationships)
Used to fetch a primary entity and populate its lookup navigation objects and multi-select bridge relations:

```csharp
public List<Employee> GetEmployeesWithDetails()
{
    using (var context = new MyDBContext())
    {
        return context.Employees
            .Include(e => e.Department)                       // 1-to-many lookup relationship
            .Include(e => e.EmployeeSkills)                   // Many-to-many bridge relationship
                .ThenInclude(es => es.Skill)                  // Deep-nest relationship load
            .ToList();
    }
}
```

---

## 2. Dynamic Simultaneous Filter
Applies multiple optional filters (e.g. from ComboBox selections) conditionally:

```csharp
public List<Employee> FilterEmployees(int selectedDeptId, int selectedSkillId)
{
    using (var context = new MyDBContext())
    {
        IQueryable<Employee> query = context.Employees
            .Include(e => e.Department)
            .Include(e => e.EmployeeSkills);

        // 1. Filter by Department (0 represents "All" option selected)
        if (selectedDeptId > 0)
        {
            query = query.Where(e => e.DepartmentId == selectedDeptId);
        }

        // 2. Filter by Skill (0 represents "All" option selected)
        if (selectedSkillId > 0)
        {
            query = query.Where(e => e.EmployeeSkills.Any(es => es.SkillId == selectedSkillId));
        }

        return query.ToList();
    }
}
```

---

## 3. Aggregation & Statistics (Exam 5 Style)
Computes counts, rates, and groupings directly in the database:

```csharp
public class StudyTaskStats
{
    // A. Count of completed tasks
    public int GetCompletedTasksCount(int studentId)
    {
        using (var context = new MyDBContext())
        {
            return context.StudyTasks
                .Count(t => t.StudentId == studentId && t.IsCompleted == true);
        }
    }

    // B. Calculate completion percentage rate
    public double GetCompletionRate(int studentId)
    {
        using (var context = new MyDBContext())
        {
            var totalTasks = context.StudyTasks.Count(t => t.StudentId == studentId);
            if (totalTasks == 0) return 0.0;

            var completedTasks = context.StudyTasks.Count(t => t.StudentId == studentId && t.IsCompleted == true);
            return Math.Round(((double)completedTasks / totalTasks) * 100, 2);
        }
    }

    // C. Group tasks by priority and count
    public Dictionary<int, int> GetTaskCountByPriority()
    {
        using (var context = new MyDBContext())
        {
            return context.StudyTasks
                .GroupBy(t => t.Priority)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
```
