using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ItemUpdater
{
    public static class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Please make sure you are inside a directory with ONLY the items you want to update");
            await Task.Delay(3000);
            Console.WriteLine("Grabbing files, hold on");
            
            string address = Assembly.GetExecutingAssembly().Location;
            address = address.Replace(address.Contains("exe") ? "ItemUpdater.exe" : "ItemUpdater.dll", "");
            
            Console.WriteLine("Address: " + address);

            foreach (var file in (await DirSearch(address)).Where(ShouldEdit))
                FilesToChange.Add(file);
            
            Console.WriteLine("Found " + FilesToChange.Count + " files to change.");
            int completedFiles = 0;
            foreach (string file in FilesToChange)
            {
                await File.AppendAllTextAsync(file,"\nExclude_From_Master_Bundle");
                completedFiles++;
                Console.WriteLine("Completed " + completedFiles + "/" + FilesToChange.Count);
            }
            
            Console.WriteLine("Done!");
            await Task.Delay(100000);

        }
        
        private static readonly List<string> FilesToChange = new List<string>();

        public static bool ShouldEdit(string fileLocation)
        {
            bool fileCheck = fileLocation.Contains(".dat") && !fileLocation.Contains("English");
            if (!fileCheck)
                return false;
            string fileText = File.ReadAllText(fileLocation).ToLower();
            return !fileText.Contains("exclude_from_master_bundle") && !fileText.Contains("npc") &&
                   !fileText.Contains("vendor");
        } 
        
        
        //Not my code below, thanks to whoever did it.
        private static async Task<List<string>> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(sDir));
                foreach (string d in Directory.GetDirectories(sDir))
                    files.AddRange(await DirSearch(d));
                
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt);
            }
        
            return files;
        }
        
        
    }
}