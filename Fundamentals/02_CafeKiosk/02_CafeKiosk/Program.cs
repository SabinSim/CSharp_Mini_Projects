using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ☕ Welcome to the Swiss Cafe Kiosk! ===");
        
        // 1. Preparation
        int totalPrice = 0;
        int number;
        bool isValid;
        bool isOrdering = true;

        // 2. Main Order Loop
        while (isOrdering)
        {
            // 3. Input Validation Loop (Trap)
            do
            {
                Console.WriteLine("\n--- 📋 Menu ---");
                Console.WriteLine("1. Americano (3 CHF)");
                Console.WriteLine("2. Cafe Latte (4 CHF)");
                Console.WriteLine("3. Pay");
                Console.Write("Enter the menu number: ");
                
                string input = Console.ReadLine();
                isValid = int.TryParse(input, out number);

                if (!isValid)
                {
                    Console.WriteLine("🚨 Invalid input! Please enter a valid number.");
                }
            } while (!isValid);

            // 4. Order Processing
            switch (number)
            {
                case 1:
                    totalPrice += 3;
                    Console.WriteLine($"✅ Americano added. (Current total: {totalPrice} CHF)");
                    break;
                case 2:
                    totalPrice += 4;
                    Console.WriteLine($"✅ Cafe Latte added. (Current total: {totalPrice} CHF)");
                    break;
                case 3:
                    Console.WriteLine($"\n💳 Order completed. Total amount to pay is {totalPrice} CHF. Thank you!");
                    isOrdering = false; // Turn off the switch to end the order
                    break;
                default:
                    Console.WriteLine("❌ Invalid menu number. Please choose 1, 2, or 3.");
                    break;
            }
        }
    }
}