using System.IO;
using No8.Solution.Interfaces.IPrinterImplementations;

namespace No8.Solution.Interfaces
{
    /// <summary>
    /// IPrinterManager interface.
    /// </summary>
    public interface IPrinterManager
    {
        /// <summary>
        /// Add printer to IPrinterManager object.
        /// </summary>
        /// <param name="printer">
        /// an instance of Printer class.
        /// </param>
        void AddPrinter(Printer printer);

        /// <summary>
        /// Print on printer from stream.
        /// </summary>
        /// <param name="printer">
        /// an instance of Printer class.
        /// </param>
        /// <param name="stream">
        /// print stream.
        /// </param>
        void Print(Printer printer, Stream stream);
    }
}
