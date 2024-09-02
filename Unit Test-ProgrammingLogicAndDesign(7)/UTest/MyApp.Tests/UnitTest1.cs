namespace MyApp.Tests;

public class UnitTest1
{
    [Fact]
    public void TotalSales()
    {
        int length = 5;
        int[] sales = new int[length];
        int total = 0;

        for (int i = 0; i < sales.Length; i++)
        {
            int temp = i;
            sales[i] = temp;
            total += temp;
        }

        Assert.Equal(10, total);
    }

    [Fact]
    public void LotteryNumber(){
        var length = 7;
        var lotteryNum = new int[length];
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            lotteryNum[i] = rnd.Next(0, 10);
        }

        var result = String.Join("", lotteryNum);
        Assert.Equal(7, result.Length);
    }
}