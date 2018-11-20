namespace No8.Solution.Interfaces
{
    /// <summary>
    /// IPrinter interface.
    /// </summary>
    public interface IPrinter
    {
        /// <summary>
        /// IPrinter name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Printer model.
        /// </summary>
        string Model { get; }
    }
}