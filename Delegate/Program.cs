namespace Delegate
{
    class Pet
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    class PetOwner
    {
        public string Name { get; set; } = "";
        public List<string> Pets { get; set; } = new();
    }

    internal class Program
    {
        static void Ex1()
        {
            int[] n1 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var nQuery = from tmp in n1
                         where (tmp % 2) == 0
                         select tmp;

            Console.WriteLine("Ex1: Even numbers");
            foreach (var n in nQuery)
                Console.WriteLine(n);
        }

        static void Ex2()
        {
            int[] n1 = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14 };
            var nQuery = from tmp in n1
                         where tmp > 0
                         where tmp < 12
                         select tmp;

            Console.WriteLine("Ex2: Positive numbers < 12");
            foreach (var n in nQuery)
                Console.WriteLine(n);
        }

        static void Ex3()
        {
            List<string> animals = new List<string> { "zebra", "elephant", "cat", "dog", "rhino", "bat" };
            var selectedAnimals = animals.Where(s => s.Length >= 5).Select(x => x.ToUpper());

            Console.WriteLine("Ex3: Animals with length >= 5, uppercased");
            foreach (var a in selectedAnimals)
                Console.WriteLine(a);
        }

        static void Ex4()
        {
            List<int> numbers = new List<int> { 6, 0, 999, 11, 443, 6, 1, 24, 54 };
            var top5 = numbers.OrderByDescending(x => x).Take(5);

            Console.WriteLine("Ex4: Top 5 numbers descending");
            foreach (var n in top5)
                Console.WriteLine(n);
        }

        static void Ex5()
        {
            Pet[] pets = {
                new Pet { Name = "Barley", Age = 8 },
                new Pet { Name = "Boots", Age = 4 },
                new Pet { Name = "Whiskers", Age = 1 }
            };

            IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            Console.WriteLine("Ex5: Pets ordered by age");
            foreach (Pet pet in query)
                Console.WriteLine($"{pet.Name} - {pet.Age}");
        }

        static void Ex6()
        {
            PetOwner[] petOwners = {
                new PetOwner { Name = "Higa", Pets = new List<string> { "Scruffy", "Sam" } },
                new PetOwner { Name = "Ashkenazi", Pets = new List<string> { "Walker", "Sugar" } },
                new PetOwner { Name = "Price", Pets = new List<string> { "Scratches", "Diesel" } },
                new PetOwner { Name = "Hines", Pets = new List<string> { "Dusty" } }
            };

            var query = petOwners
                .SelectMany(petOwner => petOwner.Pets, (petOwner, petName) => new { petOwner, petName })
                .Where(ownerAndPet => ownerAndPet.petName.StartsWith("S"))
                .Select(ownerAndPet => new
                {
                    Owner = ownerAndPet.petOwner.Name,
                    Pet = ownerAndPet.petName
                });

            Console.WriteLine("Ex6: Owners with pets starting with 'S'");
            foreach (var obj in query)
                Console.WriteLine($"{obj.Owner} - {obj.Pet}");
        }

        static void Main(string[] args)
        {
            Ex1();
            Console.WriteLine();
            Ex2();
            Console.WriteLine();
            Ex3();
            Console.WriteLine();
            Ex4();
            Console.WriteLine();
            Ex5();
            Console.WriteLine();
            Ex6();
        }
    }
}
