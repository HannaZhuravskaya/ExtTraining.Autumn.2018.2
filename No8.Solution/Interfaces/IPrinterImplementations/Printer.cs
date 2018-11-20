using System;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations
{
    public abstract class Printer : IPrinter, IEquatable<Printer>
    {
        public EventHandler<PrinterEventArgs> StartPrinting = delegate { };

        public EventHandler<PrinterEventArgs> FinishPrint = delegate { };

        protected internal Printer(string name, string model)
        {
            InputValidation(name, model);

            Name = name;
            Model = model;
        }

        public string Name { get; }

        public string Model { get; }

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

        protected virtual void OnStartPrinting(object sender, PrinterEventArgs e)
        {
            e.Info = "Printing...";
            StartPrinting?.Invoke(this, e);
        }

        protected virtual void OnFinishPrint(object sender, PrinterEventArgs e)
        {
            e.Info = "Print finished.";
            FinishPrint?.Invoke(this, e);
        }

        protected abstract void PrintDocument(Stream stream);

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