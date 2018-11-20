using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static NLog.Logger _logger;
        
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                
                CreateLogger();
                GetNamesOfExistingPrinters();
                
                var printerManager = new PrinterManager();
                var printerNames = GetNamesOfExistingPrinters();

                printerManager.OnCurrentPrint += LogPrinterManager;

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
                        ChooseAndPrint(printerManager, printerNames);
                    }
                } while (key.Key != ConsoleKey.D3);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        private static void ChooseAndPrint(PrinterManager manager, List<string> printerNames)
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
        }

        private static void CreatePrinter(PrinterManager manager, List<string> printerNames)
        {
            int nameIndex = ChoosePrinter(printerNames);

            System.Console.WriteLine("Enter printer model");
            var model = System.Console.ReadLine();

            var printer = CreateChosenPrinter(printerNames[nameIndex], model);

            if (printer != null)
            {
                manager.AddPrinter(printer);
                System.Console.WriteLine("\nPrinter added");
            }
        }

        private static List<string> GetNamesOfExistingPrinters()
        {
            var printerNames = new List<string>();

            int count = 1;
            string currentPrinter = System.Configuration.ConfigurationManager.AppSettings["printer" + count];
            while (currentPrinter != null)
            {
                printerNames.Add(currentPrinter);
                count++;
                currentPrinter = System.Configuration.ConfigurationManager.AppSettings["printer" + count];
            }

            return printerNames;
        }

        private static Printer ChoosePrinter(PrinterManager manager, List<string> printerNames)
        {
            var nameIndex = ChoosePrinter(printerNames);

            var printers = new List<Printer>(manager.GetPrinters(printerNames[nameIndex]));

            if (printers.Count == 0)
            {
                return null;
            }

            var printerIndex = ChoosePrinter(printers);

            return printers[printerIndex];
        }

        private static int ChoosePrinter<T>(List<T> printerParameters)
        {
            int nameIndex;
            do
            {
                System.Console.WriteLine("\nChoose printer name:");
                for (int i = 0; i < printerParameters.Count; ++i)
                {
                    System.Console.WriteLine($"{i + 1}. " + printerParameters[i]);
                }
            } while (!int.TryParse(System.Console.ReadLine(), out nameIndex) || nameIndex <= 0 ||
                     nameIndex > printerParameters.Count);

            return nameIndex - 1;
        }

        private static Printer CreateChosenPrinter(string printerName, string model)
        {
            Printer printer = null;
            switch (printerName.ToUpper(CultureInfo.CurrentCulture))
            {
                case "EPSON":
                    printer = new EpsonPrinter(model);
                    break;
                case "CANON":
                    printer = new CanonPrinter(model);
                    break;
            }

            return printer;
        }

        public static void LogPrinterManager(object o, PrinterEventArgs args)
        {
            _logger.Info($"Info: {args.Info}\n{args.DocumentPrintFilingTime}: {args.PrinterName} - {args.PrinterModel}");
            _logger.Debug($"{args.DocumentPrintFilingTime}: {args.PrinterName} - {args.PrinterModel}");
        }

        private static void CreateLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "logfile.txt" };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;

            _logger = NLog.LogManager.GetCurrentClassLogger();
        }
    }
}
