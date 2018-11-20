using System;
using System.Collections.Generic;
using System.IO;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution.Interfaces.IPrinterManagerImplementations
{
    public sealed class PrinterManager : IPrinterManager
    {
        private List<Printer> _printers = new List<Printer>();

        /// <summary>
        /// OnCurrent print event.
        /// </summary>
        public event EventHandler<PrinterEventArgs> OnCurrentPrint;

        /// <summary>
        /// Add printer to IPrinterManager object.
        /// </summary>
        /// <param name="printer">
        /// an instance of Printer class.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// printer must not be null.
        /// </exception>
        public void AddPrinter(Printer printer)
        {
            InputValidation(printer);

            bool isPrinterUnique = true;

            foreach (var element in _printers)
            {
                if (element.Equals(printer))
                {
                    isPrinterUnique = false;
                    break;
                }
            }

            if (isPrinterUnique)
            {
                _printers.Add(printer);
                printer.StartPrinting += OnCurrentPrint;
                printer.FinishPrint += OnCurrentPrint;
            }
        }

        /// <summary>
        /// Print on printer from stream.
        /// </summary>
        /// <param name="printer">
        /// an instance of Printer class.
        /// </param>
        /// <param name="stream">
        /// print stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// printer must not be null.
        /// </exception>
        public void Print(Printer printer, Stream stream)
        {
           InputValidation(printer);

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream) + " must not be null.");
            }

            foreach (var element in _printers)
            {
                if (element.Equals(printer))
                {
                    printer.Print(stream);
                    break;
                }
            }
        }

        /// <summary>
        /// Returns IEnumerable of printers by name.
        /// </summary>
        /// <param name="name">
        /// printer name.
        /// </param>
        /// <returns>
        /// IEnumerable of printers by name.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// name must not be null, empty or whitespace.
        /// </exception>
        public IEnumerable<Printer> GetPrinters(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name) + " must not be null, empty or whitespace");
            }

            foreach (var printer in _printers)
            {
                if (printer.Name.Equals(name))
                {
                    yield return printer;
                }
            }
        }

        /// <summary>
        /// Returns IEnumerable of printers.
        /// </summary>
        /// <returns>
        /// IEnumerable of printers.
        /// </returns>
        public IEnumerable<Printer> GetPrinters()
        {
            foreach (var printer in _printers)
            {
                yield return printer;
            }
        }

        private void InputValidation(Printer printer)
        {
            if (printer == null)
            {
                throw new ArgumentNullException(nameof(printer) + " must not be null.");
            }
        }
    }
}
