using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeDir
{
    public class RecTree
    {
        public RecTree(string startDir)
        {
            StartDir = startDir;
        }

        public string StartDir { get; set; }
        public void Print(string startDir)
        {
            SearchTree(new DirectoryInfo(startDir));
           
        }

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
        public static string ReturnSize(long size)
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
        private void SearchTree (DirectoryInfo root)
        {

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
          

            
            // Получаем все файлы в текущем каталоге
            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if(files != null)
            {
                foreach (FileInfo file in files)
                {
                    Console.WriteLine("├── {0} ({1})", file.Name, ReturnSize(file.Length));
                }
                //получаем все подкаталоги
                subDirs = root.GetDirectories();
                //проходим по каждому подкаталогу
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    Console.WriteLine("└──{0} ({1})", dirInfo.Name);
                    //РЕКУРСИЯ
                    SearchTree(dirInfo);
                }
            }
        }
    }
}
