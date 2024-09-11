using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static void Main(string[] args)
    {
        int[][] inputs = {
            new int[] {1, 2, 3, 4},
            new int[] {1, 2},
            new int[] {2, 2},
            new int[] {-4, -1, -9, 1, -7},
            new int[] {25, 50, 75, 100, 400}
        };

        int[] k = {5, 3, 4, -3, 425};

        for (int i = 0; i < inputs.Length; i++)
        {
            Console.WriteLine($"{i + 1}.\tArray: [{string.Join(", ", inputs[i])}]");
            Console.WriteLine($"\tk: {k[i]}");

            int[] result = FindSumBinarySearch(inputs[i], k[i]);
            Console.WriteLine($"\n\tResult: [{string.Join(", ", result)}]");
            Console.WriteLine('-' + new string('-', 100) + '\n');
        }
    }


// O(n^2) time | O(1) space
    public static int[] FindSum(int[] nums, int target)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                if (nums[i] + nums[j] == target)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[0]; 
    }

    public static int[] FindSumTwoPointers(int[] nums, int target)
    {
        Array.Sort(nums);
        Console.Write("Element are sorted: ");
        for(int i = 0; i < nums.Length; i++)
        {            
            Console.Write(nums[i] + " ");
        }
        int left = 0;
        int right = nums.Length - 1;

        while (left < right)
        {
           int sum = nums[left] + nums[right];
           if(sum == target)
           {
                return new int[] { left, right };
              }
              else if (sum < target)
              {
                left++;
              }
              else
              {
                right--;
           }
        }

        return new int[0];
    }

    public static int BinarySearch(int[] nums, int target)
    {
        int left = 0;
        int right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (nums[mid] == target)
            {
                return mid;
            }
            else if (nums[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return -1;
    }

    public static int[] FindSumBinarySearch(int[] nums, int target)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            int index = BinarySearch(nums, complement);
            if (index != -1 && index != i)
            {
                return new int[] { i, index };
            }
        }

        return new int[0];
    }
}
