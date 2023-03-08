// See https://aka.ms/new-console-template for more information
class List
{
    public int n;
    public int[] a;
    public List()
    {
        a=new int[0];
        n = 0;
    }
    public List(int num)
    {
        n = num;
        a=new int[num+1];
        Console.WriteLine("请在此键入数组，一行一个数字");
        for(int i=1;i<=n;i++)
        {
            a[i] = int.Parse(Console.ReadLine());
        }
    }
    public void FindKey()
    {
        int sum = 0;
        int avg = 0;
        int maxx = -0xfffffff;
        int minn = 0xfffffff;
        for(int i=1;i<=n; i++)
        {
            sum += a[i];
            maxx = (a[i] > maxx) ? a[i] : maxx;
            minn = (a[i]<minn) ? a[i] : minn;
        }
        avg = sum / n;
        Console.WriteLine("这串数字的和是: "+sum+"\n这串数字的平均值是: "+avg+"\n这段数字的最小值和最大值分别是: "+minn+" "+maxx+"\n");
    }
}
namespace ListOfNumbers
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            int n;
            Console.WriteLine("请在此键入数组的元素个数");
            n= int.Parse(Console.ReadLine());
            List listt = new List(n);
            listt.FindKey();
        }
    }
}
