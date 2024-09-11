
class Program {
    public static void Main(string[] args) {        
        List<List<int>> nums1 = new List<List<int>>
        {
            new List<int> { 23, 33, 35, 41, 44, 47, 56, 91, 105 },
            new List<int> { 1, 2 },
            new List<int> { 1, 1, 1 },
            new List<int> { 6 },
            new List<int> { 12, 34, 45, 56, 67, 78, 89, 99 }
        };

        List<List<int>> nums2 = new List<List<int>>
        {
            new List<int> { 32, 49, 50, 51, 61, 99 },
            new List<int> { 7 },
            new List<int> { 1, 2, 3, 4 },
            new List<int> { -99, -45 },
            new List<int> { 100 }
        };

        for (int i = 0; i < nums1.Count; i++)
        {
            Console.WriteLine($"{i + 1}.\tFirst array: {string.Join(", ", nums1[i])}");
            Console.WriteLine($"\tSecond array: {string.Join(", ", nums2[i])}");

            int m = nums1[i].Count;
            int n = nums2[i].Count;
            var mergedArray = ThreePointers(nums1[i], m, nums2[i], n);
            Console.WriteLine($"\tMerged array: {string.Join(", ", mergedArray)}");
            Console.WriteLine(new string('-', 100));
        }
    }

public static List<int> ThreePointers(List<int> nums1, int m, List<int> nums2, int n)
  {
    if (n == 0) return nums1;
    for (int a = 0; a < n; a++)
        {
            nums1.Add(0);
        }
    int i = m - 1;
    int j = n - 1;
    int k = m + n - 1;
    while (i >= 0 && j >= 0)
    {
      if (nums1[i] > nums2[j])
      {
        nums1[k--] = nums1[i--];
      }
      else
      {
        nums1[k--] = nums2[j--];
      }
    }

    while (j >= 0)
    {
      nums1[k--] = nums2[j--];
    }
    return nums1;
  }  

}
    

