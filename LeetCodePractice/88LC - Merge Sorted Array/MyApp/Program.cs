using System;

public class Program
{
    public static T[] MergeArrays<T>(T[] array1, T[] array2)
    {        
        return array1.Concat(array2).ToArray(); 
    }

    
    public static void Merge(int[] nums1, int m, int[] nums2, int n) {
        // Array.Resize(ref nums1, m);
        // Array.Resize(ref nums2, n);
        // int[] rerult = MergeArrays(nums1, nums2);
        // Array.Sort(rerult);
        // foreach (var item in rerult)
        // {
        //     Console.Write(item + " ");
        // }
        int len1 = m - 1;
        int len2 = n - 1;
        int resultLen = m + n - 1;
        for (int i = 0; i > nums1.Length - 1; i++)
        {
            nums1[m + i] = nums2[i];
        }

        foreach (var item in nums1)
        {
             Console.Write(item + " ");
         }
    }


    // nums1 = [1,2,3,0,0,0], m = 3, 
    //nums2 = [2,5,6], n = 3
    public static void Main(string[] args)
    {
        int[] array1 = { 1, 2, 3, 0, 0, 0 }; int m = 3;
        int[] array2 = { 2, 5, 6 }; int n = 3;
        Merge(array1, m, array2, n);
        
    }
}
