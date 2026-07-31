using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    public delegate void ScoreUpdateHandler(string studentName, double newScore);

    public interface IEvaluatable
    {
        double AverageScore { get; }
        string GetRank();
    }

    public abstract class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public List<double> Scores { get; set; }

        protected Student(string studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
            Scores = new List<double>();
        }

        public void AddScore(double score)
        {
            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10");
            }
            Scores.Add(score);
        }

        public abstract string GetRank();
    }

    public class UndergraduateStudent : Student, IEvaluatable
    {
        public string Major { get; set; }

        public UndergraduateStudent(string studentId, string fullName, string major)
            : base(studentId, fullName)
        {
            Major = major;
        }

        public double AverageScore => Scores.Count == 0 ? 0 : Scores.Average();

        public override string GetRank()
        {
            var avg = AverageScore;
            if (avg >= 8.5) return "Excellent";
            if (avg >= 7.0) return "Good";
            if (avg >= 5.0) return "Average";
            return "Fail";
        }
    }

    public class ScoreManager
    {
        private List<UndergraduateStudent> _students;
        private ScoreUpdateHandler _onScoreUpdated;

        public ScoreManager(ScoreUpdateHandler handler)
        {
            _students = new List<UndergraduateStudent>();
            _onScoreUpdated = handler;
        }

        public void AddStudent(UndergraduateStudent student)
        {
            _students.Add(student);
        }

        public void AddScoreToStudent(string studentId, double score)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student not found: {studentId}");
            }

            student.AddScore(score);
            _onScoreUpdated?.Invoke(student.FullName, score);
        }

        public List<UndergraduateStudent> GetTopStudents(int n)
        {
            return _students.OrderByDescending(s => s.AverageScore).Take(n).ToList();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ScoreUpdateHandler handler = (name, score) =>
                Console.WriteLine($"[SCORE UPDATE]: {name} received score: {score}");

            var manager = new ScoreManager(handler);

            var s1 = new UndergraduateStudent("S1", "Nguyen Van A", "IT");
            var s2 = new UndergraduateStudent("S2", "Tran Thi B", "BA");
            var s3 = new UndergraduateStudent("S3", "Le Van C", "CS");

            manager.AddStudent(s1);
            manager.AddStudent(s2);
            manager.AddStudent(s3);

            manager.AddScoreToStudent("S1", 8.5);
            manager.AddScoreToStudent("S1", 9.0);
            manager.AddScoreToStudent("S2", 7.0);
            manager.AddScoreToStudent("S2", 7.5);
            manager.AddScoreToStudent("S3", 6.0);
            manager.AddScoreToStudent("S3", 4.5);

            try
            {
                manager.AddScoreToStudent("S1", 11.0); // Triggers ArgumentOutOfRangeException
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n=== Top 2 Students ===");
            var top = manager.GetTopStudents(2);
            for (int i = 0; i < top.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {top[i].FullName} | Major: {top[i].Major} | Average: {top[i].AverageScore:F2} | Rank: {top[i].GetRank()}");
            }
        }
    }
}
