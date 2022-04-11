using System;

namespace TreeDir
{
    static class Program
    {
        static void Main(string[] args)
        {
            string startDir = null;
            for (int i = 0;i < args.Length;i++)
            {
                string arg = args[i];
                if(arg == "-h")
                {
                 RecTree d = new RecTree(startDir);
                    d.Print(args[i+1]);
                }
            }
        }
    }
}
