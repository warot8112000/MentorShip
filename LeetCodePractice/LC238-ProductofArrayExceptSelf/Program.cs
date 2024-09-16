class Program
{
    static void Main()
    {
       int[][] inputs = 
        {
            new int[] {1, 2, 3, 4},
            new int[] {5, -3, 1, 2},
            new int[] {2, 2, 2, 0},
            new int[] {0, 0, 0, 0},
            new int[] {-1, -2, -4}
        };

        for (int i = 0; i < inputs.Length; i++)
        {
            Console.WriteLine($"{i + 1}.\t Array: {String.Join(", ", inputs[i])}");
            Console.WriteLine($"\n\t List of products: {String.Join(", ", ProductExceptSelf3(inputs[i]))}");
            Console.WriteLine(new string('-', 70) + '\n');
        }
    }

    public static int[] ProductExceptSelf(int[] nums)
    {
        int n = nums.Length;
        int[] output = new int[n];
        
        for (int i = 0; i < n; i++)
        {
            int product = 1;
            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    product *= nums[j];
                }
            }
            output[i] = product;
        }

        return output;
    }

    public static int[] ProductExceptSelf2(int[] nums)
    {
        int n = nums.Length;
        int[] output = new int[n];
        int[] prefix = new int[n];
        int[] suffix = new int[n];

         prefix[0] = 1;
         suffix[n-1] = 1;

        for(int i = 1; i < n; i++)
        {
            prefix[i] = prefix[i-1] * nums[i-1];
        }
        for(int i = n-2; i >= 0; i--)
        {
            suffix[i] = suffix[i+1] * nums[i+1];
        }

        for(int i = 0; i < n; i++)
        {
            output[i] = prefix[i] * suffix[i];
        }
        

        return output;
    }
    
    public static int[] ProductExceptSelf3(int[] nums)
    {
        int n = nums.Length;
        int[] output = new int[n];
        int prefix = 1;
        int suffix = 1;
        for(int i = 0; i < n; i++)
        {
            output[i] = prefix; //output[0] = 1; output[1] = 1*1 prefix =1*1*2; 
            prefix *= nums[i];
        }
        for(int i = n-1; i >= 0; i--)
        {
            output[i] *= suffix;
            suffix *= nums[i];
        }

        return output;
    }
}