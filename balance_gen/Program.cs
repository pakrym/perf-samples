using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            for (int k = 0; k < 1000; k++)
            {
                var p = new Stack<char>();
                var stringBuilder = new StringBuilder();
                var random = new Random();

                for (int i = 0; i < 100000; i++)
                {
                    if (random.Next(2) == 0 || p.Count == 0)
                    {
                        var next = kinds[random.Next(kinds.Length)];
                        stringBuilder.Append(next.Item1);
                        p.Push(next.Item2);
                    }
                    else
                    {
                        stringBuilder.Append(p.Pop());
                    }
                }
                stringBuilder.Append(Environment.NewLine);
                File.AppendAllText("out.txt", stringBuilder.ToString());
            }
        }
    }
}
