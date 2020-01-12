using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ItemUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please make sure you are inside a directory with ONLY the items you want to update");
            Console.WriteLine("Grabbing files, hold on");
            string address = Assembly.GetExecutingAssembly().Location.Replace("ItemUpdater.dll", "");
            //Later allow for other types to be used
            foreach (var file in DirSearch(address).Where(file => file.Contains(".dat") && !file.Contains("English")))
            {
                _filesToChange.Add(file);
            }
            Console.WriteLine("Found " + _filesToChange.Count + " files to change.");
            int completedFiles = 0;
            foreach (String file in _filesToChange)
            {
                File.AppendAllText(file,"\nExclude_From_Master_Bundle");
                completedFiles++;
                Console.WriteLine("Completed " + completedFiles + "/" + _filesToChange.Count);
            }
            
            Console.WriteLine("Done!");
            
        }
        
        private static List<String> _filesToChange = new List<string>();
        
        
        //Not my code below, thanks to whoever did it.
        private static List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt);
            }
        
            return files;
        }
        
        
    }
}