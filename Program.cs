using System.IO;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace FileSystemWatcherAufgabe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using FileSystemWatcher fsw = new FileSystemWatcher(@"D:\Lookup", "*.lookup");
            #region nicht benötigte Trigger
            //fsw.Changed += OnCreated;
            //fsw.Deleted += OnDeleted;
            //fsw.Renamed += OnRenamed;
            //fsw.Error += OnError;
            #endregion
            fsw.Created += OnCreated;
            //fsw.Filter = "*.lookup";
            fsw.IncludeSubdirectories = false;
            fsw.EnableRaisingEvents = true;     //muss true sein, da FileSystemWatcher sonst nicht aktiv überwacht
            Console.WriteLine("Warte auf  eine zu überwachende Datei");
            Console.ReadLine();
        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader(e.FullPath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        IPHostEntry ipHostEntry = Dns.GetHostEntry(line);
                        IPAddress[] arr = ipHostEntry.AddressList;
                        foreach(IPAddress ip in arr)
                        {
                            Console.WriteLine(ip); 
                        }
                        File.AppendAllText(@"D:\Lookup\" +line+ ".resolve", string.Join<IPAddress>("\n", arr));
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message);
                {
                    switch (ex.Message)
                    {
                        case "Value cannot be null. (Parameter 'hostName')":
                            Console.WriteLine("Datei wurde angelegt, hat aber keinen Inhalt");
                        break;
                        case "":
                            break;
                    }
                }
            }
            finally
            {
                
            }
        }
    }
}
