using System;
using System.IO;
using System.Linq;
using No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations;
using No8.Solution.Interfaces.IPrinterManagerImplementations;
using NUnit.Framework;

namespace No8.Solution.Tests
{
    [TestFixture]
    class PrinterManagerTests
    {
        [TestCase("model1", ExpectedResult = 1)]
        [TestCase("model1-1-1-1234", "model1", ExpectedResult = 2)]
        public int AddPrinters_ValidPrinters_AddedPrinters(params string[] models)
        {
            var manager = new PrinterManager();
            var random = new Random();

            foreach (var model in models)
            {
                if (random.Next() % 2 == 0)
                {
                    manager.AddPrinter(new CanonPrinter(model));
                }
                else
                {
                    manager.AddPrinter(new EpsonPrinter(model));
                }
            }

            return manager.GetPrinters().ToArray().Length;
        }

        [Test]
        public void AddPrinters_InvalidPrinters_ExpectedArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new PrinterManager().AddPrinter(null));

        [Test]
        public void Print_InvalidPrinter_ExpectedArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() =>
                new PrinterManager().Print(null, new FileStream("a.txt", FileMode.Create)));

        [Test]
        public void Print_InvalidStream_ExpectedArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new PrinterManager().Print(new EpsonPrinter("a"), null));
    }
}