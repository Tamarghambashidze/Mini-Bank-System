using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Extentsions
{
    public static class ExtensionMethods
    {
        public static string Hash(this string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }
        public static bool Verify(this string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
        public static int IntParse(this string text)
        {
            int result = default;
            do
            {
                try
                {
                    result = int.Parse(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    text = Console.ReadLine();
                }
            } while (result == default);
            return result;
        }
        public static decimal DecimalParse(this string text)
        {
            decimal result = default;
            do
            {
                try
                {
                    result = decimal.Parse(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    text = Console.ReadLine();
                }
            } while (result == default);
            return result;
        }
        public static void ReadFromFile(this string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
        public static string FilePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FinalProject/Log Information";
            Directory.CreateDirectory(path);
            return Path.Combine(path, "bank_system_log.txt");
        }
        public static void AddToFile(this string content)
        {
            string path = FilePath();
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(content);
            }
        }
    }
}
