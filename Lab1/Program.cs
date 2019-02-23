using System;
using System.Configuration;
using System.IO;

namespace Lab1
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"IS Lab1 Продувнова В. А. ИДМ-17-06{Environment.NewLine}");
            try
            {
                string src;
                Check(out src, true);
                string dst;
                Check(out dst, false);
                Processor.Process(src, dst);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void Check(out string s, bool isSrc)
        {
            while (true)
            {
                Console.WriteLine(
                    $"Введите путь до {(isSrc ? "исходной" : "целевой")} папки (в случае пустой строки будет использован путь по умолчанию):");
                s = Console.ReadLine();
                if (string.IsNullOrEmpty(s))
                {
                    s = isSrc
                        ? ConfigurationManager.AppSettings.Get("sourceDest")
                        : ConfigurationManager.AppSettings.Get("resultDest");
                    if (!Directory.Exists(s) || Directory.GetFiles(s, "*.csv").Length == 0)
                    {
                        if (isSrc)
                        {
                            Console.WriteLine(!Directory.Exists(s)
                                ? "Исходная папка по умолчанию не существует, введите путь до исходных файлов"
                                : "Исходная папка по умолчанию пуста, введите путь до исходных файлов");
                            continue;
                        }
                        try
                        {
                            Directory.CreateDirectory(s);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(
                                $"Произошла непредвиденная ошибка: {e.Message}{Environment.NewLine}Попробуйте повторить ввод");
                            continue;
                        }
                    }
                    Console.WriteLine($"Используется вариант по умолчанию: {s}");
                    break;
                }                
                if (!Directory.Exists(s))
                    Console.WriteLine("Введён несуществующий путь, повторите попытку");
                else
                {
                    Console.WriteLine("Путь введён");
                    break;
                }
            }
        }
    }
}
