using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    // 1. DELEGATE
    public delegate void ScoreUpdateHandler(string studentName, double newScore);

    // 2. INTERFACE
    public interface IEvaluatable
    {
        double AverageScore { get; }
        string GetRank();
    }

    // 3. CLASSES
    
    // Abstract Class Student
    public abstract class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public List<double> Scores { get; set; }

        public Student(string studentId, string fullName)
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

    // Class UndergraduateStudent
    public class UndergraduateStudent : Student, IEvaluatable
    {
        public string Major { get; set; }

        public UndergraduateStudent(string studentId, string fullName, string major) 
            : base(studentId, fullName)
        {
            Major = major;
        }

        public double AverageScore
        {
            get
            {
                if (Scores.Count == 0) return 0;
                return Scores.Average();
            }
        }

        public override string GetRank()
        {
            double avg = AverageScore;
            if (avg >= 8.5) return "Excellent";
            if (avg >= 7.0) return "Good";
            if (avg >= 5.0) return "Average";
            return "Fail";
        }
    }

    // Class ScoreManager
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
                throw new InvalidOperationException("Student not found: " + studentId);
            }

            student.AddScore(score);
            _onScoreUpdated?.Invoke(student.FullName, score);
        }

        public List<UndergraduateStudent> GetTopStudents(int n)
        {
            return _students
                .OrderByDescending(s => s.AverageScore)
                .Take(n)
                .ToList();
        }
    }

    // 4. TESTING (Main Method)
    public class Program
    {
        public static void Main(string[] args)
        {
            // Callback prints score updates to console
            ScoreUpdateHandler handler = (studentName, newScore) => 
                Console.WriteLine($"[SCORE UPDATE]: {studentName} received score: {newScore:F1}");

            ScoreManager manager = new ScoreManager(handler);

            // 1. Create at least 3 UndergraduateStudent objects
            var s1 = new UndergraduateStudent("S1", "Nguyen Van A", "IT");
            var s2 = new UndergraduateStudent("S2", "Tran Thi B", "BA");
            var s3 = new UndergraduateStudent("S3", "Le Van C", "GD");

            // 2. Add each student to a ScoreManager instance
            manager.AddStudent(s1);
            manager.AddStudent(s2);
            manager.AddStudent(s3);

            // 3. Add multiple scores to students
            manager.AddScoreToStudent("S1", 8.5);
            manager.AddScoreToStudent("S2", 7.0);
            manager.AddScoreToStudent("S3", 6.0);

            // Add score that triggers ArgumentOutOfRangeException
            try
            {
                manager.AddScoreToStudent("S1", 11.5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Format message to grab the message portion
                string message = ex.Message.Split('\n')[0]; // Grabs the clean message
                if (message.Contains("Score must be between 0 and 10"))
                {
                    Console.WriteLine("Error: Score must be between 0 and 10");
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine();

            // 4. Call GetTopStudents(2) and print results
            Console.WriteLine("=== Top 2 Students ===");
            var topStudents = manager.GetTopStudents(2);
            int rank = 1;
            foreach (var student in topStudents)
            {
                Console.WriteLine($"{rank++}. {student.FullName,-14} | Major: {student.Major} | Average: {student.AverageScore:F2} | Rank: {student.GetRank()}");
            }
        }
    }
}