using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLog;
using No8.Solution.Interfaces.IPrinterImplementations;
using No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations;
using No8.Solution.Interfaces.IPrinterManagerImplementations;

namespace No8.Solution.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var printerManager = new PrinterManager();
            var printerNames = new List<string>() { EpsonPrinter.Name, CanonPrinter.Name };

            printerManager.OnIsBeingPrinted += Logger;
            printerManager.OnPrinted += Logger;


            System.ConsoleKeyInfo key;
            do
            {
                System.Console.WriteLine("\nSelect your choice:");
                System.Console.WriteLine("1:Add new printer");
                System.Console.WriteLine("2:Choose printer and print");
                System.Console.WriteLine("3:Exit");

                key = System.Console.ReadKey();

                if (key.Key == ConsoleKey.D1)
                {
                    CreatePrinter(printerManager, printerNames);
                }

                if (key.Key == ConsoleKey.D2)
                {
                    Print(printerManager, printerNames);
                }


            } while (key.Key != ConsoleKey.D3);
        }

        public static void Logger(object o, PrinterEventArgs args)
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "logfile.txt" };
            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;

            var logger = NLog.LogManager.GetCurrentClassLogger();

            logger.Info($"Info: {args.Info}\n{args.DocumentPrintFilingTime}: {args.PrinterName} - {args.PrinterModel}");
            logger.Debug($"{args.DocumentPrintFilingTime}: {args.PrinterName} - {args.PrinterModel}");
        }

        private static void Print(PrinterManager manager, List<string> printerNames)
        {
            var chosenPrinter = ChoosePrinter(manager, printerNames);
            if (chosenPrinter == null)
            {
                System.Console.WriteLine("There is no printer you choose.");
                return;
            }
           
            var o = new OpenFileDialog();
            o.ShowDialog();
            var path =  o.FileName;

            if (!string.IsNullOrWhiteSpace(path))
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    manager.Print(chosenPrinter, stream);
                } 
            } 
           // PrinterManager.Log("Printed on Epson");
        }

        private static Printer ChoosePrinter(PrinterManager manager, List<string> printerNames)
        {
            System.Console.WriteLine("\nChoose printer name:");
            for (int i = 0; i < printerNames.Count; ++i)
            {
                System.Console.WriteLine($"{i+1}. " + printerNames[i]);
            }

            int.TryParse(System.Console.ReadLine(), out var nameIndex);
            if (nameIndex > 0 && nameIndex < printerNames.Count + 1)
            {
                var printers = new List<Printer>(manager.GetPrinters(printerNames[nameIndex - 1]));

                if (printers.Count == 0)
                {
                    return null;
                }

                for (int i = 0; i < printers.Count; ++i)
                {
                    System.Console.WriteLine($"{i + 1}. {printers[i]}");
                }
               
                int.TryParse(System.Console.ReadLine(), out var printerIndex);
                if (printerIndex > 0 && printerIndex < printers.Count + 1)
                {
                    return printers[printerIndex - 1];
                }
            }

            return null;
        }

        private static void CreatePrinter(PrinterManager manager, List<string> printerNames)
        {
            System.Console.WriteLine("\nChoose printer name:");
            for (int i = 0; i < printerNames.Count; ++i)
            {
                System.Console.WriteLine($"{i + 1}. " + printerNames[i]);
            }

            int.TryParse(System.Console.ReadLine(), out var nameIndex);
            if (nameIndex > 0 && nameIndex < printerNames.Count + 1)
            {
                System.Console.WriteLine("Enter printer model");
                var model = System.Console.ReadLine();
                Printer printer = null;
                switch (nameIndex - 1)
                {
                    case 0:
                        printer = new EpsonPrinter(model);
                        break;
                    case 1:
                        printer = new CanonPrinter(model);
                        break;     
                }

                if (printer != null)
                {
                    manager.AddPrinter(printer);
                }

                System.Console.WriteLine("\nPrinter added");
            }
        }
    }
}
