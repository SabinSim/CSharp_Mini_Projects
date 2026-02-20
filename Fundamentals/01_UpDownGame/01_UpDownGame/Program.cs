using System;

class Program
{
	static void Main()
	{
		bool isValid;
		int number;
		int secretNumber = new Random().Next(1, 101);
		
		bool isCorrect = false;

		while (!isCorrect)
		{
			do
			{
				Console.Write("Enter a number: ");
				string input = Console.ReadLine();
			
				isValid = int.TryParse(input, out number);

				if (!isValid)
				{
					Console.WriteLine("Invalid input");
				}
			
			} while (!isValid);

			if (number > secretNumber)
			{
				Console.WriteLine("Down");
			}
			else if (number < secretNumber)
			{
				Console.WriteLine("Up");
			}
			else
			{
				Console.WriteLine("Right");
				isCorrect = true;
			}
			
		}

		
	}
}