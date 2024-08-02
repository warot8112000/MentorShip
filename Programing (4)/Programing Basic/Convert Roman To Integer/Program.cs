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