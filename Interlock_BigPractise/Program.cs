using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interlock_BigPractise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            object _locker = new object();
            int turn = 0;


            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (_locker)
                    {
                        while (turn == 1)
                            Monitor.Wait(_locker);
                        turn = 1;
                        Monitor.Pulse(_locker);
                        Console.WriteLine("Пінг походив");
                    }
                }
            });
            thread1.Start();

            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (_locker)
                    {
                        while (turn == 0)
                            Monitor.Wait(_locker);
                        turn = 0;
                        Monitor.Pulse(_locker);
                        Console.WriteLine("Понг походив");
                    }
                }
            });
            thread2.Start();

            thread1.Join();
            thread2.Join();



            // ==========================

            Console.WriteLine("\n\n=====================================\n\n");
            _locker = new object();
            turn = 0;


            Thread thread3 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (_locker)
                    {
                        turn = 1;
                        Console.WriteLine("Пінг походив");
                    }
                }
            });
            thread3.Start();

            Thread thread4 = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (_locker)
                    {
                        turn = 0;
                        Console.WriteLine("Понг походив");
                    }
                }
            });
            thread4.Start();

            thread3.Join();
            thread4.Join();


            // ==========================

            Console.WriteLine("\n\n=====================================\n\n");
            _locker = new object();
            int currentRunner = 1;
            int currentLap = 1;

            Console.Write($"N (3 - 10) = ");
            int n = int.TryParse(Console.ReadLine(), out n) ? n : 10;

            Thread[] threads = new Thread[n];

            for (int i = 1; i <= n; i++)
            {
                int id = i;
                threads[i - 1] = new Thread(() =>
                {
                    lock (_locker)
                    {
                        while (currentLap < 3)
                        {
                            while (currentRunner != id)
                                Monitor.Wait(_locker);
                            Console.WriteLine($"Бігун {id} пробіг коло {currentLap}");
                            currentRunner = (id % n) + 1;

                            if (currentRunner == 1) currentLap++;
                            Monitor.PulseAll(_locker);
                        }
                    }
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
