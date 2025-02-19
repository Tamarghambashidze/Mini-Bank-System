using FinalProject.Extentsions;
using FinalProject.Repsitories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    internal class FileManagement
    {
        private FileSystem _system;
        public FileManagement(FileSystem system)
        {
            _system = system;
        }
        public void FileManagementSystem()
        {
            do
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("File management");
                Console.Write("1. Generate an account statement\n2. " +
                    "View system logs\n3. Exit\n Answer: ");
                int answer = Console.ReadLine().IntParse();
                switch (answer)
                {
                    case 1:
                        Console.WriteLine("Account statement");
                        _system.AccountStatement();
                        break;
                    case 2:
                        Console.WriteLine("View system logs");
                        _system.SystemLog();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Incorrect input");
                        continue;
                }
            } while (true);
        }
    }
}
