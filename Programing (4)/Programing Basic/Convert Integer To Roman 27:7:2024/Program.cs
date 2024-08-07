
string romanNumber;
int number;
do
{
    Console.WriteLine("Enter a roman number: ");
    romanNumber = Console.ReadLine();
} while (!IsIntegerValid(romanNumber, out number));

romanNumber = ConvertIntegerToRoman(number);
Console.WriteLine($"The roman number is: {romanNumber}");

string ConvertIntegerToRoman( int number)
{

    string[] romanNumbers = new string[] { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
    int[] numbers = new int[] { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
    string result = "";

    for (int i = romanNumbers.Count() - 1; i >= 0; i--)
    {
        while (number >= numbers[i])
        {
            result += romanNumbers[i];
            number -= numbers[i];
        }
    }

    return result;
}

bool IsIntegerValid(string? romanNumber, out int number)
{
    
    if (int.TryParse(romanNumber, out number))
    {
        if (number < 1 || number > 3999)
    {
        return false;
    }
        return true;
    }
    return false;
}