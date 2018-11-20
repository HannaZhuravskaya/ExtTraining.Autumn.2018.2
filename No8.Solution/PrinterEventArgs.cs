using System;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution
{
    public class PrinterEventArgs : EventArgs
    {
   /// <summary>
   /// Document print filling time.
   /// </summary>
        public readonly DateTime DocumentPrintFilingTime;

        /// <summary>
        ///  Printer name.
        /// </summary>
        public readonly string PrinterName;

        /// <summary>
        /// Printer model.
        /// </summary>
        public readonly string PrinterModel;

        /// <summary>
        /// Initializes a new instance of PrinterEventArgs.
        /// </summary>
        /// <param name="printer">
        /// Class Printer object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// printer must not be null.
        /// </exception>
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

        /// <summary>
        /// Some info.
        /// </summary>
        public string Info { get; set; }
    }
}
