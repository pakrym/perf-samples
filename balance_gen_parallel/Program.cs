using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace balance
{
    class Program
    {
        static void Main(string[] args)
        {
            var kinds = new (char, char)[]
            {
                ('[', ']'),
                ('(', ')'),
                ('{', '}'),
                ('<', '>'),
            };

            var tasks = new List<Task>();
            var queue = new Queue<string>();
            var lineCount = 1000;

            tasks.Add(Task.Run(() =>
            {
                var linesLeft = lineCount;
                while (true)
                {
                    lock (queue)
                    {
                        if (queue.Count == 0)
                        {
                            Thread.SpinWait(20);
                            continue;
                        }

                        File.AppendAllText("out.txt", queue.Dequeue());

                        linesLeft--;
                        if (linesLeft == 0)
                        {
                            break;
                        }
                    }
                }
            }));

            var producerCount = 5;

            for (int p = 0; p < producerCount; p++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int line = 0; line < lineCount / producerCount; line++)
                    {
                        lock (queue)
                        {
                            var rest = new Stack<char>();
                            var stringBuilder = new StringBuilder();
                            var random = new Random();

                            for (int i = 0; i < 100000; i++)
                            {
                                if (random.Next(2) == 0 || rest.Count == 0)
                                {
                                    var next = kinds[random.Next(kinds.Length)];
                                    stringBuilder.Append(next.Item1);
                                    rest.Push(next.Item2);
                                }
                                else
                                {
                                    stringBuilder.Append(rest.Pop());
                                }
                            }

                            stringBuilder.Append(Environment.NewLine);
                            queue.Enqueue(stringBuilder.ToString());
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            if (args.FirstOrDefault() == "wait")
            {
                Console.ReadLine();
            }
        }
    }
}
