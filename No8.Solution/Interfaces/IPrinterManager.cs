using System.IO;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution.Interfaces
{
    public interface IPrinterManager
    {
        void AddPrinter(Printer printer);

        void Print(Printer printer, Stream stream);
    }
}
