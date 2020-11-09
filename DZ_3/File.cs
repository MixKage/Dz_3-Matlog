using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_3
{
    public class FileMeneger
    {
        public StreamWriter sw;
        public FileMeneger(string Path = "Answers.txt")
        {
            sw = new StreamWriter(File.Open(Path, FileMode.Append));
            sw.WriteLine("============================");
        }
        public void StringSave(string text)
        {
            sw.WriteLine(text);
        }
        public void FileClose()
        {
            sw.Close();
        }
    }
}
