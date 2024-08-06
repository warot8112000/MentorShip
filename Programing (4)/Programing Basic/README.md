
## Table of Contents
- [1. Convert Roman To Numbers](#1-convert-roman-to-numbers)
- [2. Convert Integer to Roman](#2-convert-integer-to-roman)

## 1. Convert Roman To Numbers
Solution: When you enter Roman letters. We will have 2 cases: 

- **Case 1**:  A smaller value appears before a larger value . We'll do the subtraction (IV = -1 + 5)
- **Case 2**: A smaller value appears after or equal to a larger value. We'll do addition (III = 1 + 1 + 1)

Additionally, before converting from Roman to Integer, check that it is a valid Roman numeral.

```csharp
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

partial class Program
{
    static void Main(string[] args)
    {   string romanNumber;
        int number;
        do
        {
            Console.Write("Enter a Roman number: ");
            romanNumber = Console.ReadLine()?.Trim();
        } while (!IsRomanNumberValid(romanNumber));

        number = RomanToInt(romanNumber);
        Console.WriteLine($"The Roman number {romanNumber} is equal to {number} in decimal.");

    }

    private static bool IsRomanNumberValid(string romanNumber)
    {
        if (string.IsNullOrEmpty(romanNumber))
        {
            return false;
        }
        string pattern = "^M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$";
        return Regex.IsMatch(romanNumber, pattern, RegexOptions.IgnoreCase);
    }

    static int RomanToInt(string s)
    {
        var map = new Dictionary<char, int>
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };

        int num = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (i + 1 < s.Length && map[s[i]] < map[s[i + 1]])
            {
                num -= map[s[i]];
            }
            else
            {
                num += map[s[i]];
            }
        }
        return num;
    }
}
```

## 2. Convert Integer to Roman

The program uses two arrays: romanNumbers, which contains Roman numeral symbols ordered from smallest to largest, and numbers, which contains the corresponding values of those symbols.

For each symbol, if the input number is greater than or equal to the value of that symbol, the program adds the symbol to the result string and subtracts the value of that symbol from the number.

```csharp
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

