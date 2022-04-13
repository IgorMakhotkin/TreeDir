using System;

namespace TreeDir
{
    static class Program
    {
        static void ShowParametrs()
        {
            Console.WriteLine("Usage: FsTree -p [dir] [-h] [-o] [dirOutput] [-q]");
            Console.WriteLine(" -q (--quite) - признак вывода сообщений в стандартный поток вывода (если указана, то не выводить лог в консоль. Только в файл)");
            Console.WriteLine(" -p (--path) - путь к папке для обхода (по-умолчанию текущая папка вызова программы)");
            Console.WriteLine(" -o (--output) - путь к тестовому файлу, куда записать результаты выполнения расчёта (по-умолчанию файл sizes-YYYY-MM-DD.txt в текущей папке вызова программы)");
            Console.WriteLine(" -h (--humanread) - признак формирования размеров файлов в человекочитаемой форме (размеры до 1Кб указывать в байтах, размеры до 1Мб в килобайтах с 2 знаками после запятой, размеры до 1Гб в мегабайтах с 2 знаками после запятой, размеры до 1Тб - в Гб с 2 знаками после запятой)");
        }
        static void Main(string[] args)
        {
            string startDir = null;
            bool printInFile = false;
            bool returnHumanRead = false;
            string output = null;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg == "-q" || arg == "--quite")
                {
                    printInFile = true;
                }
                else if (arg == "-h" || arg == "--humanread")
                {
                    returnHumanRead = true;
                }
                else if (arg == "-o" || arg == "--output")
                {
                    output = args[i + 1];
                    i++;
                    if (String.IsNullOrEmpty(output))
                    {
                        output = ".";
                    }
                }
                else if (arg == "-p" || arg == "--path")
                {
                    startDir = args[i + 1];
                    i++;
                }
                else
                {
                    Console.WriteLine("Неизвестный параметр {0}", arg);
                    ShowParametrs();
                    return;
                }
            }
            if (String.IsNullOrEmpty(startDir))
            {
                startDir = ".";
            }

            new RecTree(startDir,output)
            {
                PrintInFile = printInFile,
                ReturnHumanRead = returnHumanRead

            }.Print();
            Console.WriteLine(output);
        }
    }
}
