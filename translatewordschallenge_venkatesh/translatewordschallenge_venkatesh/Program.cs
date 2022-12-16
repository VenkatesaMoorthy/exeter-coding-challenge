using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace translatewordschallenge_venkatesh
{
    class Program
    {
        static void Main(string[] args)
        {
            var memory_utilisation = 0.0;
            using (Process proc = Process.GetCurrentProcess())
            {
                //string file_root_path = "C:\\Users\\M9016170\\source\\repos\\translatewordschallenge_venkatesh";
                string current_path = System.IO.Directory.GetCurrentDirectory();
                string file_root_path = Directory.GetParent(Directory.GetParent(Directory.GetParent(current_path).FullName).FullName).FullName;

                //---------------------------------START PROCESS--------------------------------------
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                // --------------- READ 1000 WORDS --------------------------------------------
                string[] find_words = System.IO.File.ReadAllLines(file_root_path + "\\Input\\find_words.txt");

                // --------------- READ SHAKESPEARE PARAGRAPH --------------------------------------------
                string shakespeare = System.IO.File.ReadAllText(file_root_path + "\\Input\\t8.shakespeare.txt");
                string translated_shakespeare = shakespeare;

                //string shakespeare = System.IO.File.ReadAllText(file_root_path + "\\Output\\t8.shakespeare.translated.txt");

                // --------------- READ FRENCH DICTIONARY --------------------------------------------
                //var dictinory = File.ReadAllLines(file_root_path + "\\Input\\french_dictionary.csv")
                //    .Select(line => line.Split(',')).ToDictionary(r => r[0], r => r[1]);

                var dictinory_file_array = File.ReadAllLines(file_root_path + "\\Input\\french_dictionary.csv");

                Dictionary<string, string> dictinory = new Dictionary<string, string>();
                foreach (var s in dictinory_file_array)
                {
                    int index = s.IndexOf(',');
                    dictinory.Add(s.Substring(0, index), s.Substring(index + 1));
                }


                var frequency_csv = new StringBuilder();

                int i = 1;
                foreach (var word in find_words)
                {

                    int freq = Regex.Matches(translated_shakespeare, @"(?<![\w])" + word + @"(?![\w])", RegexOptions.IgnoreCase).Count;

                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    var french_word = dictinory.Where(r => r.Key == word).Select(x => x.Value).FirstOrDefault().ToString();

                    translated_shakespeare = Regex.Replace(translated_shakespeare, @"(?<![\w])" + word + @"(?![\w])", french_word, RegexOptions.IgnoreCase);


                    //int lower_freq = Regex.Matches(translated_shakespeare, @"(?<![\w])" + ti.ToLower(word) + @"(?![\w])").Count;
                    //translated_shakespeare = Regex.Replace(translated_shakespeare, @"(?<![\w])" + ti.ToLower(word) + @"(?![\w])", ti.ToLower(french_word));

                    //int upper_freq = Regex.Matches(translated_shakespeare, @"(?<![\w])" + ti.ToUpper(word) + @"(?![\w])").Count;
                    //translated_shakespeare = Regex.Replace(translated_shakespeare, @"(?<![\w])" + ti.ToUpper(word) + @"(?![\w])", ti.ToUpper(french_word));

                    //int title_freq = Regex.Matches(translated_shakespeare, @"(?<![\w])" + ti.ToTitleCase(word) + @"(?![\w])").Count;
                    //translated_shakespeare = Regex.Replace(translated_shakespeare, @"(?<![\w])" + ti.ToTitleCase(word) + @"(?![\w])", ti.ToTitleCase(french_word));

                    var frequency_newline = string.Format("{0},{1},{2}", word, french_word, freq);
                    frequency_csv.AppendLine(frequency_newline);

                    Console.WriteLine(i + " The word " + word + " total : " + freq + " times ---- Equvalant French Word : " + french_word);
                    i = i + 1;
                }

                // --------------- WRITE  into frequency.csv--------------------------------------------
                if (File.Exists(file_root_path + "\\Output\\frequency.csv"))
                {
                    File.Delete(file_root_path + "\\Output\\frequency.csv");
                }
                File.WriteAllText(file_root_path + "\\Output\\frequency.csv" , frequency_csv.ToString());


                // --------------- WRITE  into t8.shakespeare.translated.txt --------------------------------------------
                if (File.Exists(file_root_path + "\\Output\\t8.shakespeare.translated.txt"))
                {
                    File.Delete(file_root_path + "\\Output\\t8.shakespeare.translated.txt");
                }
                File.WriteAllText(file_root_path + "\\Output\\t8.shakespeare.translated.txt", translated_shakespeare);



                // --------------- WRITE PROCESS EXECUTION TIME AND MEMORY UTILISATION into performance.txt--------------------------------------------
                watch.Stop();
                var time_taken = watch.ElapsedMilliseconds;
                memory_utilisation = proc.PrivateMemorySize64 / (1024 * 1024);
                List<string> perfornamce = new List<string>();
                perfornamce.Add("Time to process : " + TimeSpan.FromMilliseconds(time_taken).TotalMinutes.ToString() + " minutes "+ TimeSpan.FromMilliseconds(time_taken).TotalSeconds.ToString() + " seconds");
                perfornamce.Add("Memory used :" + memory_utilisation + " MB");
                if (File.Exists(file_root_path + "\\Output\\performance.txt"))
                {
                    File.Delete(file_root_path + "\\Output\\performance.txt");
                }
                File.WriteAllLines(file_root_path + "\\Output\\performance.txt", perfornamce);
            }
        }
    }
}