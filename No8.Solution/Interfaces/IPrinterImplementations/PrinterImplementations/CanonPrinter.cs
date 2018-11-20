using System;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations
{
    public sealed class CanonPrinter : Printer
    {
        public new static string Name = "Canon";

        public CanonPrinter(string model) : base(Name, model)
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
