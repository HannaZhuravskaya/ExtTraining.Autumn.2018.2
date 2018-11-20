using System;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations
{
    public abstract class Printer : IPrinter, IEquatable<Printer>
    {
        /// <summary>
        /// Calling events at the beginning of printing.
        /// </summary>
        public EventHandler<PrinterEventArgs> StartPrinting = delegate { };

        /// <summary>
        /// Calling an event when printing is completed.
        /// </summary>
        public EventHandler<PrinterEventArgs> FinishPrint = delegate { };

        /// <summary>
        /// Initializes a new instance of Printer.
        /// </summary>
        /// <param name="name">
        /// Printer name.
        /// </param>
        /// <param name="model">
        /// Printer model.
        /// </param>
        /// <exception cref="ArgumentException">
        /// name and model must not be null, empty or whitespace.
        /// </exception>
        protected internal Printer(string name, string model)
        {
            InputValidation(name, model);

            Name = name;
            Model = model;
        }

        /// <summary>
        /// Printer name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Printer model.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Print from stream.
        /// </summary>
        /// <param name="stream">
        /// print stream.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// stream must not be null.
        /// </exception>
        public void Print(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream) + " must not be null.");
            }

            var args = new PrinterEventArgs(this) { Info = "Printing..." };
            OnStartPrinting(this, args);

            PrintDocument(stream);
            
            OnFinishPrint(this, args);
        }

        public override string ToString()
        {
            return $"{Name} - {Model}";
        }

        public bool Equals(Printer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name) && string.Equals(Model, other.Model);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Printer)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Model != null ? Model.GetHashCode() : 0);
            }
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
        protected abstract void PrintDocument(Stream stream);

        private void OnStartPrinting(object sender, PrinterEventArgs e)
        {
            e.Info = "Printing...";
            StartPrinting?.Invoke(this, e);
        }

        private void OnFinishPrint(object sender, PrinterEventArgs e)
        {
            e.Info = "Print finished.";
            FinishPrint?.Invoke(this, e);
        }

        private void InputValidation(string name, string model)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name) + " must not be null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException(nameof(model) + " must not be null, empty or whitespace.");
            }
        }
    }
}