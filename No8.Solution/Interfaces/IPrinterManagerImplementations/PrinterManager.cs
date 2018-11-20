using System;
using System.Collections.Generic;
using System.IO;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution.Interfaces.IPrinterManagerImplementations
{
    public sealed class PrinterManager : IPrinterManager
    {
        private List<Printer> _printers = new List<Printer>();

        public event EventHandler<PrinterEventArgs> OnIsBeingPrinted;

        public event EventHandler<PrinterEventArgs> OnPrinted;

        public void AddPrinter(Printer printer)
        {
            _printers.Add(printer);
            printer.PrintStarted += OnIsBeingPrinted;
            printer.PrintCompleted += OnPrinted;
        }

        public void Print(Printer printer, Stream stream)
        {
            foreach (var element in _printers)
            {
                if (element.Equals(printer))
                {
                    printer.Print(stream);
                }
            }
        }

        public IEnumerable<Printer> GetPrinters(string name)
        {
            foreach (var printer in _printers)
            {
                if (printer.Name.Equals(name))
                {
                    yield return printer;
                }
            }
        }
    }
}
