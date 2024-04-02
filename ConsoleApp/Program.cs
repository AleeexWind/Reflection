// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using System.Diagnostics;

Console.WriteLine("Reflection lesson!");
Console.WriteLine("");
Console.WriteLine("");

//Replace by your path to the FclassAsCSV.txt file
string path = @"D:\Applications\Projects\OtusReflection\Reflection\ConsoleApp\FclassAsCSV.txt";

int count = 100000;
Stopwatch stopwatch = new();

stopwatch.Start();

string mySerializedString = string.Empty;
for (int i = 0; i < count; i++)
{
    mySerializedString = Serializer.SerializeToCsv<F>(F.Get());
}
stopwatch.Stop();
Console.WriteLine(mySerializedString);
Console.WriteLine("");
Console.WriteLine("Time elapsed: {0}", stopwatch.ElapsedMilliseconds);
Console.WriteLine("");
Console.WriteLine("");


stopwatch.Restart();
stopwatch.Start();

string standartSerializedString = string.Empty;
for (int i = 0; i < count; i++)
{
    standartSerializedString = Newtonsoft.Json.JsonConvert.SerializeObject(F.Get());
}
stopwatch.Stop();
Console.WriteLine(standartSerializedString);
Console.WriteLine("");
Console.WriteLine("Time elapsed: {0}", stopwatch.ElapsedMilliseconds);
Console.WriteLine("");
Console.WriteLine("");


List<string> linesOfSCV = Serializer.ReadFromTheFileByLines(path);

stopwatch.Restart();
stopwatch.Start();
for (int i = 0; i < count; i++)
{
    var ff = Serializer.DeserializeCSV<F>(linesOfSCV);
}
stopwatch.Stop();
Console.WriteLine("My deserialization time elapsed: {0}", stopwatch.ElapsedMilliseconds);
Console.WriteLine("");
Console.WriteLine("");

stopwatch.Restart();
stopwatch.Start();
for (int i = 0; i < count; i++)
{
    var fff = Newtonsoft.Json.JsonConvert.DeserializeObject<F>(standartSerializedString);
}
stopwatch.Stop();
Console.WriteLine("Newtonsoft deserialization time elapsed: {0}", stopwatch.ElapsedMilliseconds);
Console.WriteLine("");
Console.WriteLine("");


Console.ReadLine();
