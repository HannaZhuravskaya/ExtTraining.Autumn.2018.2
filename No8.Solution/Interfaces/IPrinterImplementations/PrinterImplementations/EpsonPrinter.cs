﻿using System;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations
{
    /// <summary>
    /// The implementation of Printer class.
    /// </summary>
    public sealed class EpsonPrinter : Printer
    {
        /// <summary>
        /// Name of printer.
        /// </summary>
        private const string PrinterName = "Epson";

        /// <summary>
        /// Initializes a new instance of EpsonPrinter.
        /// </summary>
        /// <param name="model">
        /// Printer model.
        /// </param>
        public EpsonPrinter(string model) : base(PrinterName, model)
        {
        }

        /// <summary>
        /// Logic of printing.
        /// </summary>
        /// <param name="stream">
        /// print stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// stream must not be null.
        /// </exception>
        protected override void PrintDocument(Stream stream)
        {
            InputValidation(stream);

            for (int i = 0; i < stream.Length; i++)
            {
                Console.WriteLine(stream.ReadByte());
            }
        }

        private void InputValidation(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream) + " must not be null.");
            }
        }
    }
}