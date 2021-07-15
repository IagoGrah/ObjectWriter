using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ObjectWriter
{
    class Program
    {
        // Settings
        static string wordsFilePath = $"words.txt";
        static string fileNameFormat = "{0} - {1}";
        static string dateFormat = "dd-MM-yy";
        static string filePath = "Journal/";
        static int timerSeconds = 600;

        static void Main(string[] args)
        {
            LoadSettings();

            var dateString = DateTime.Today.ToString(dateFormat);
            var word = GenerateWord();
            var fileName = String.Format(fileNameFormat, dateString, word);

            System.IO.Directory.CreateDirectory(filePath);
            var path = $"{filePath}{fileName}.txt";
            
            File.WriteAllText(path, $"{word}\n\n");
            Process.Start("notepad.exe", path);

            CountDown();

            Console.WriteLine("Time's up!");
            Thread.Sleep(3000);
            Environment.Exit(0);
        }

        static string GenerateWord()
        {
            string[] lines = File.ReadAllLines(wordsFilePath);

            Random rand = new Random();
            var word = lines[rand.Next(lines.Length)];
            word = Char.ToUpper(word[0]) + word.Substring(1);
            
            return word;
        }

        static void CountDown()
        {
            for (int a = timerSeconds; a >= 0; a--)
            {
                var time = $"{(a/60).ToString("D2")}:{(a%60).ToString("D2")}";

                Console.WriteLine("Write about the given word");
                Console.Write("Time remaining: {0:D2}", time);
                Thread.Sleep(1000);
                Console.Clear();
            }
        }

        static void LoadSettings()
        {
            var lines = File.ReadAllLines("settings.txt").Where(x => x.Contains('=')).Select(x => x.Split('=')).ToDictionary((x => x[0]), (x => x[1].Trim('"')));
            
            wordsFilePath = lines["wordsFilePath"];
            dateFormat = lines["dateFormat"];
            fileNameFormat = lines["fileNameFormat"];
            filePath = lines["filePath"];
            timerSeconds = Int32.Parse(lines["timerSeconds"]);
        }
    }
}
