using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NinkyNonk.Shared.Environment;

namespace ItemUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Project.LoggingProxy.LogProgramInfo();
        Project.LoggingProxy.Log("Please make sure you are inside a directory with ONLY the items you want to update");
        await Task.Delay(3000);
        Project.LoggingProxy.LogInfo("Grabbing files...");
            
        string address = Assembly.GetExecutingAssembly().Location;
        address = address.Replace(address.Contains("exe") ? "ItemUpdater.exe" : "ItemUpdater.dll", "");
            
        Project.LoggingProxy.LogUpdate("Address: " + address);

        foreach (var file in (await DirSearch(address)).Where(ShouldEdit))
            FilesToChange.Add(file);
            
        Project.LoggingProxy.LogUpdate("Found " + FilesToChange.Count + " files to change.");
        int completedFiles = 0;
        foreach (string file in FilesToChange)
        {
            await File.AppendAllTextAsync(file,"\nExclude_From_Master_Bundle");
            completedFiles++;
            Project.LoggingProxy.LogUpdate("Completed " + completedFiles + "/" + FilesToChange.Count);
        }
            
        Project.LoggingProxy.LogSuccess("Done!");
        Console.ReadKey();
    }
        
    private static readonly List<string> FilesToChange = new();

    private static bool ShouldEdit(string fileLocation)
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
            Project.LoggingProxy.LogError(excpt.Message);
        }
        
        return files;
    }
    
}