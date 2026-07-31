namespace LAB3 {
    class Calculator {

        public void Calculate<T1, T2>(T1 a, T2 b) {
            if (IsNumeric(a) && IsNumeric(b)) {
                dynamic val1 = a;
                dynamic val2 = b;

                Console.WriteLine($"Addition: {val1 + val2}");
                Console.WriteLine($"Subtraction: {val1 - val2}");
                Console.WriteLine($"Multiplication: {val1 * val2}");
                Console.WriteLine($"Division: {val1 / val2}");
            }
            else Console.WriteLine("Error: One or both inputs are not numeric.");
        }

        private bool IsNumeric(object val)
        {
            return val is int || val is double || val is float || val is decimal;
        }
    }
}
