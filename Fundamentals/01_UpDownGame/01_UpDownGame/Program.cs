using System;

class Program
{
	static void Main()
	{
		bool isValid;
		int number;

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
		
		Console.WriteLine("Passed : " + number);
	}
}