using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebApiHost
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsDebug();
        }

        [Conditional("DEBUG")]
        private static void RunAsDebug()
        {
            Debug();
        }

        private static void Debug()
        {
            if (WebApiHostWinService.Start())
            {
                Console.WriteLine("启动成功.");
                Console.WriteLine("退出请输入:/q");
                string input;
                while ((input = Console.ReadLine()) != "/q")
                {
                    if (input == "cls")
                    {
                        Console.Clear();
                    }
                }
                WebApiHostWinService.Close();
            }
            else
            {
                Console.WriteLine("启动失败.");
                Console.ReadKey();
            }
        }
    }
}
