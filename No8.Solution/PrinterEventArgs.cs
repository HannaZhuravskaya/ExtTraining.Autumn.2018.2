using System;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution
{
    public class PrinterEventArgs : EventArgs
    {
        public readonly DateTime DocumentPrintFilingTime;

        public readonly string PrinterName;

        public readonly string PrinterModel;

        internal PrinterEventArgs(Printer printer)
        {
            this.DocumentPrintFilingTime = DateTime.Now;

            if (printer == null)
            {
                throw new ArgumentNullException(nameof(printer) + " must not be null.");
            }

            this.PrinterModel = printer.Model;
            this.PrinterName = printer.Name;
        }

        public string Info { get; set; }
    }
}
