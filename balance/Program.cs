using System;
using System.IO;

namespace balance
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var streamReader = new StreamReader(File.OpenRead("in.txt")))
            {
                while (!streamReader.EndOfStream)
                {
                    var s = streamReader.ReadLine();

                    while (true)
                    {
                        var prevLen = s.Length;
                        s = s.Replace("{}", "");
                        s = s.Replace("[]", "");
                        s = s.Replace("<>", "");
                        s = s.Replace("()", "");
                        if (s.Length == 0)
                        {
                            // empty - success
                            Console.WriteLine("true");
                            break;
                        }
                        if (prevLen == s.Length)
                        {
                            // length didn't change - unbalanced
                            Console.WriteLine("false");
                            break;
                        }
                    }

                }
            }
        }
    }
}
