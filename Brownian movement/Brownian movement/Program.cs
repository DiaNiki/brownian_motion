using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brownian_movement
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                return;
            }
            int n = int.Parse(args[0]);
            int k = int.Parse(args[1]);
            double p = double.Parse(args[2]);

            object[] locks = new object[n + 1];
            for (int i = 0; i <= n; ++i)
            {
                locks[i] = new object();
            }
            int[] cells = new int[n + 1];
            cells[0] = k;

            //Barrier startIteration = new Barrier(k + 1);
            //Barrier endIteration = new Barrier(k + 1);

            Thread[] threads = new Thread[k];
            Random rnd = new Random();
            for (int i = 0; i < k; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    int index = 0;
                    for (; ; )
                    {
                        //startIteration.SignalAndWait();
                        lock (cells)
                        {
                            //lock (locks[index])
                            {
                                --cells[index];
                            }
                            if (rnd.NextDouble() < p)
                            {
                                index = Math.Min(index + 1, n);
                            }
                            else
                            {
                                index = Math.Max(index - 1, 0);
                            }
                            //lock (locks[index])
                            {
                                ++cells[index];
                            }
                        }
                        //endIteration.SignalAndWait();
                    }
                });
                threads[i].Start();
            }

            //startIteration.SignalAndWait();

            for (int iter = 0; iter < 12; ++iter)
            {
                //endIteration.SignalAndWait();
                lock (cells)
                {
                    for (int i = 0; i <= n; ++i)
                    {
                        Console.Write(cells[i] + " ");
                    }
                }
                Console.WriteLine();
                Thread.Sleep(5000);
                //startIteration.SignalAndWait();
            }

            foreach (var thread in threads)
            {
                thread.Abort();
            }

            int k_res = 0;
            for (int i = 0; i <= n; ++i)
            {
                k_res += cells[i];
            }
            Console.WriteLine("Number of atoms in the end: " + k_res);

            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}
        }
    }
}
