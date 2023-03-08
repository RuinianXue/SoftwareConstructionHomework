// See https://aka.ms/new-console-template for more information
class Matrix
{
    public int[][] a;
    public int n, m;
    public Matrix()
    {
        a = new int[2][];
        n=m = 0;
    }
    public Matrix(int n, int m)
    {
        this.n = n;
        this.m = m;
        a = new int[n][];
        for (int i = 0; i < n; i++)
        {
            string s=Console.ReadLine();
            int j = 0,pos=0;
            a[i]=new int[m];
            while (pos < s.Length)
            {
                if (s[pos] <= '9' && s[pos]>='0')
                {
                    int num = 0;
                    while(pos<s.Length&& s[pos] <= '9' && s[pos] >= '0')
                    {
                        num = num * 10 + (int)(s[pos]-'0');
                        pos++;
                    }
                    
                    a[i][j] = num;
                    //Console.WriteLine(i+" "+j);
                    j++;
                }
               else pos++;
            }
        }
    }
    public void outAns()
    {
        for (int i = 0;i < n;i++)
        {
            for(int j = 0;j < m;j++)
            {
                Console.Write(a[i][j]+" ");
            }
            Console.Write("\n");
        }
    }
    public bool checkMatrix()
    {
        for(int i= 0;i < m; i++)
        {
            for(int j = 1;j < n&&(i+j<m); j++)
            {
                if (a[0][i] != a[j][i+j]) return false;
            }
        }
        return true;
    }
}

namespace CheckMatrix
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("请用两行分别键入矩阵的长和宽");
            int n, m;
            n=int.Parse(Console.ReadLine());
            m=int.Parse(Console.ReadLine());
            Console.WriteLine("请键入矩阵");
            Matrix m1 = new Matrix(n,m);
            m1.outAns();
            Console.WriteLine(m1.checkMatrix());
        }
    }
}
/*
2
3
1 2 3
4 1 1
 */