using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvokeIR.PowerForensics;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            InvokeIR.PowerForensics.Main.getInode(@"C:\Windows\system32\config\SAM");
            Console.ReadLine();
        }
    }
}
