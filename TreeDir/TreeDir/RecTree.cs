using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDir
{
    public class RecTree
    {
        public RecTree(string startDir,string output)
        {
            StartDir = startDir;
            Output = output;
            
        }
        DateTime date = DateTime.Now;
        
        public string StartDir { get; set; }
        public string Output = null;

        public bool PrintInFile { get; set; } = false;
        public bool ReturnHumanRead { get; set; } = false;
        public string defaultOutputName()
        {
            DateTime date = DateTime.Now;
            string name = "sizes-" + date.Year + "-" + date.Month + "-" + date.Day + ".txt";
            return name;
        }
        public void Print()
        {
            if(PrintInFile)
            {
                Output += defaultOutputName();
                printDirInFile(Output, StartDir);
            }
            else
            {
                WriteColored(StartDir, DirColor);
                WriteLine();
            }
            PrintTree(StartDir);
        }
        public Action<string> Write { get; set; } = Console.Write;
        public Action<ConsoleColor> SetColor { get; set; } = color => Console.ForegroundColor = color;
        public ConsoleColor DefaultColor { get; set; } = Console.ForegroundColor;
        public ConsoleColor DirColor { get; set; } = ConsoleColor.Blue;
        public ConsoleColor FileColor { get; set; } = Console.ForegroundColor;

        // Конвертирование байтов
        public static string byteconverter(long bytes, string suffix, bool useSuffix = false, int digits = 0)
        {
            List<string> values = new List<string>() { "B", "KB", "MB", "GB", "TB" };
            try
            {
                return string.Format(@"{0}{1}", Math.Round(bytes / Math.Pow(1024, values.IndexOf(suffix)), digits), useSuffix ? suffix : "");
            }
            catch (Exception ex) { return ex.Message; }
        }
        // формирования размеров файлов в человекочитаемой форме
        public static string ReturnSizeHM(long size)
        {
            if (size < 1000) // размеры до 1Кб указывать в байтах
                return byteconverter(size, "B", true, 0);
            else if (size < 1000000) //размеры до 1Мб в килобайтах с 2 знаками после запятой
                return byteconverter(size, "KB", true, 2);
            else if (size < 1000000000)// размеры до 1Гб в мегабайтах с 2 знаками после запятой
                return byteconverter(size, "MB", true, 2);
            else if (size < 1000000000000)// размеры до 1Тб - в Гб с 2 знаками после запятой
                return byteconverter(size, "GB", true, 2);
            else
                return size.ToString();
        }
        public string ReturnSizeByte(long size)
        {
            return byteconverter(size, "B", true, 0);
        }
        public string ReturnSize(long Length)
        {
            if (ReturnHumanRead)
            {
                return ReturnSizeHM(Length);
            }
            else
                return ReturnSizeByte(Length);
        }
        private void WriteLine(string text = "")
        {
            Write(text + Environment.NewLine);
        }
        private void WriteColored(string text, ConsoleColor color, string size)
        {
            SetColor(color);
            Write(text);
            SetColor(DefaultColor);
            Write("(" + size + ")");
        }
        private void WriteColored(string text, ConsoleColor color)
        {
            SetColor(color);
            Write(text);
            SetColor(DefaultColor);
        }
        private ConsoleColor GetColor(FileSystemInfo fsItem)
        {
            if (fsItem.IsDirectory())
            {
                return DirColor;
            }
            return FileColor;
        }
        private void WriteName(FileSystemInfo fsItem, string size)
        {
            WriteColored(fsItem.Name, GetColor(fsItem), size);
        }
        private void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, GetColor(fsItem));
        }
        private void printInFile(string Output,FileSystemInfo fsItem,FileInfo file)
        {
            StreamWriter result = new StreamWriter(Output, true);
            result.Write(fsItem.Name);
            result.Write(" ");
            result.Write(ReturnSize(file.Length));
            result.Close();
        }
        private void printInFile(string Output, FileSystemInfo fsItem)
        {
            StreamWriter result = new StreamWriter(Output, true);
            result.Write(fsItem.Name);
            result.Close();
        }
        private void printInFile(string Output, string prefix)
        {
            StreamWriter result = new StreamWriter(Output, true);
            result.Write(prefix);
            result.Close();
        }
        private void printInFile(string Output)
        {
            StreamWriter result = new StreamWriter(Output, true);
            result.WriteLine();
            result.Close();
        }
        private void printDirInFile(string Output,string startdir)
        {
            StreamWriter result = new StreamWriter(Output, true);
            result.WriteLine(startdir);
            result.Close();
        }
        private void PrintTree(string startDir, string prefix = "")
        {
            try
            {
                var di = new DirectoryInfo(startDir);
                var fsItems = di.GetFileSystemInfos()
                    .Where(f => !f.Name.StartsWith("."))
                    .OrderBy(f => f.Name)
                    .ToList();

                // Проходим список
                foreach (var fsItem in fsItems.Take(fsItems.Count - 1))
                {
                    if (PrintInFile)
                    {
                        printInFile(Output, prefix + "├── ");
                    }
                    else
                        Write(prefix + "├── ");

                    // если не является директорией
                    if (!fsItem.IsDirectory())
                    {
                        // получаем информацию о нужном нам файле
                        FileInfo[] files = null;
                        files = di.GetFiles(fsItem.Name);
                        foreach (FileInfo file in files)
                        {
                            //Печатаем имя файла и его размер
                            if (PrintInFile)
                            {
                                printInFile(Output, fsItem, file);
                                printInFile(Output);
                            }
                            else
                            {
                                WriteName(fsItem, ReturnSize(file.Length));
                                WriteLine();
                            }
                        }
                    }
                    //Если является директорией 
                    if (fsItem.IsDirectory())
                    {
                        //Печатаем имя папки
                        if (PrintInFile)
                        {
                            printInFile(Output, fsItem);
                            printInFile(Output);
                        }
                        else
                        {
                            WriteName(fsItem);
                            WriteLine();
                        }
                        //Рекурсивно вызываем функцию передавая имя текущей папки
                        PrintTree(fsItem.FullName, prefix + "│   ");
                    }
                }
                // для последнего элемента в списке
                var lastFsItem = fsItems.LastOrDefault();
                if (lastFsItem != null)
                {
                    if (PrintInFile)
                    {
                        printInFile(Output, prefix + "└── ");
                    }
                    else
                    {
                        Write(prefix + "└── ");
                    }
                    FileInfo[] files = null;
                    files = di.GetFiles(lastFsItem.Name);
                    foreach (FileInfo file in files)
                    {
                        if (PrintInFile)
                        {
                            printInFile(Output, lastFsItem, file);
                            printInFile(Output);
                        }
                        else
                        {
                            WriteName(lastFsItem, ReturnSize(file.Length));
                            WriteLine();
                        }
                    }
                    if (lastFsItem.IsDirectory())
                    {
                        if (PrintInFile)
                        {
                            printInFile(Output, lastFsItem);
                            printInFile(Output);
                        }
                        else
                        {
                            WriteName(lastFsItem);
                            WriteLine();
                        }
                        PrintTree(lastFsItem.FullName, prefix + "    ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public static class FileSystemInfoExtensions
    {
        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
    }
}

