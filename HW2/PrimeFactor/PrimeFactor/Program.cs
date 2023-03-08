// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using static System.Net.Mime.MediaTypeNames;

class Prime// 筛选素数
{
    public int[] primeList;
    public int maxNum;
    public int num;
    public int[] primeNums;
    public Prime()
    {
        primeList = new int[1];
        maxNum = 0;
        primeNums = new int[1];
        num = 0;
    }
    public Prime(int num)
    {
        maxNum = num;
        primeList = new int[num+1];
        primeNums = new int[num + 1];
        for (int i = 1; i <= num; i++)
        {
            primeList[i] = 1;
        }
        int sqrtn = (int)(System.Math.Sqrt(num));
        for (int i = 2; i <= sqrtn; i++)
        {
            int j = i+i;
            while (j <= num)
            {
                primeList[j] = 0;
                j += i;
               // Console.Write(j + " ");
            }

        }
        
    }
    public void getPrime()
    {
        for (int i = 2; i <= maxNum; i++)
        {
            //Console.Write(primeList[i] + " ");
            if (primeList[i] == 1)
            {
                primeNums[++num] = i;
               // Console.Write(i+ " ");
            }
        }
    }
    public void output()
    {
        Console.Write(maxNum+"内的质数有：");
        for (int i = 1; i <= num; i++)
        {
            Console.Write(primeNums[i] + " ");
        }
        Console.Write( "\n");
    }
}

namespace PrimeFactor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            int inputNum = int.Parse(Console.ReadLine());
            Prime primeUnderNum = new Prime(inputNum);
            primeUnderNum.getPrime();
            primeUnderNum.output();

            Console.Write(inputNum+"的质因数有：");
            for (int i = 1; i <= primeUnderNum.num; i++)
            {
                if (inputNum % primeUnderNum.primeNums[i] == 0)
                {
                    Console.Write(primeUnderNum.primeNums[i] + " ");
                }
            }
            Console.Write("\n");
            //Console.Read();
        }
    }
}
