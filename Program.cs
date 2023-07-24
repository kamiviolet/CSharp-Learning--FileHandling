using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

// Use Directory.getCurrentDirectory() to get the current working directory
// Use Path.Combine(pathSegment_1, pathSegment_2...) to ensure the correct format for different OS

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

// Use Directory.CreateDirectory(path) to create a new directory

Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "stores", "205"));

// Use Directory.Exists(path) to check if a directory exists

bool doesDirectoryExist = Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "stores", "205"));

// Use File.ReadAllText(filepath) to read the file content (return as string by default)

var salesJson = File.ReadAllText($"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales.json");

var salesTotalDir = Path.Combine(Directory.GetCurrentDirectory(), "salesTotalDir");

Directory.CreateDirectory(salesTotalDir);

// add package Newtonsoft.Json locally, call the method Json.Convert.DeserializeObject<T>(JSON-like string)
var salesData = JsonConvert.DeserializeObject<SalesTotal>(salesJson);

// Use File.WriteAllText(filepath, content<string>) to write to the file
// So you need to use ToString() to be able to print the content to File.WriteAllText()
File.WriteAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", salesData.Total.ToString());

// Use Environment.NewLine to add a new line after content
// Use Path.DirectorySeparatorChar to ensure the correct format for different OS
File.WriteAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", $"{salesData.Total}{Environment.NewLine}");

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalaesTotal(salesFiles);

// Use File.AppendAllText(filepath, content) to avoid overwrite the whole file with new content
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

/*
 * FindFiles take a directory name as argument
 * create an Enumerable of string
 * use Directory.EnumerateFiles(folderName, fileFormat, options)
 * to search through the directory to get all files
 * which fulfill conditions inside the directory
 * and add them to the Enumerable as result
 */

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }
    return salesFiles;
}

/* 
 * CalculateSalesTotal takes an Enumerable of string as argument
 * Loop through the element(file) inside the Enumerable
 * Read them one by one
 * Parse the content to Json and get the number
 * Sum up the number inside the file content
 */


double CalculateSalaesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        var data = File.ReadAllText(file);
        SalesData? salesStat = JsonConvert.DeserializeObject<SalesData?>(data);
        salesTotal += salesStat?.Total ?? 0;
    }

    return salesTotal;
}

record SalesData(double Total);

class SalesTotal
{
    public double Total { get; set; }
}