using System;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations
{
    public sealed class EpsonPrinter : Printer
    {
        public new static readonly string Name = "Epson";

        public EpsonPrinter(string model) : base(Name, model)
        {
        }

        protected override void PrintDocument(Stream stream)
        {
            for (int i = 0; i < stream.Length; i++)
            {
                Console.WriteLine(stream.ReadByte());
            }
        }
    }
}
