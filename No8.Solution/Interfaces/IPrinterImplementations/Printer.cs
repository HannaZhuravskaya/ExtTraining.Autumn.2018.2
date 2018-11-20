using System;
using System.Collections.Generic;
using System.IO;

namespace No8.Solution.Interfaces.IPrinterImplementations
{
    public abstract class Printer : IPrinter, IEquatable<Printer>
    {
        public EventHandler<PrinterEventArgs> PrintStarted = delegate { };
        public EventHandler<PrinterEventArgs> PrintCompleted = delegate { };

        private Queue<Stream> _printQueueStreams = new Queue<Stream>();
        private Queue<PrinterEventArgs> _printQueueEventArgs = new Queue<PrinterEventArgs>();

        //private bool _isDisposed = false;

        private bool _isWorking = false;

        protected internal Printer(string name, string model)
        {
            Name = name;
            Model = model;
        }

        public string Name { get; }

        public string Model { get; }

        public void Print(Stream stream)
        {
            _printQueueStreams.Enqueue(stream);
            _printQueueEventArgs.Enqueue(new PrinterEventArgs(this));

            if (!_isWorking)
            {
                _isWorking = true;
                if (_printQueueStreams.Count != 0)
                {
                    var args = _printQueueEventArgs.Dequeue();
                    args.Info = "Printing...";
                    OnPrintStarted(this, args);
                    PrintDocument(_printQueueStreams.Dequeue());
                    args.Info = "Printing finished.";
                    OnPrintCompleted(this, args);
                }

                _isWorking = false;
            }
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

            return this.Equals((Printer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Model != null ? Model.GetHashCode() : 0);
            }
        }

        protected virtual void OnPrintStarted(object sender, PrinterEventArgs e)
        {
            PrintStarted?.Invoke(this, e);
        }

        protected virtual void OnPrintCompleted(object sender, PrinterEventArgs e)
        {
            PrintCompleted?.Invoke(this, e);
        }

        protected abstract void PrintDocument(Stream stream);

        /*public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        
        ~Printer()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Dispose(false);
            }
        }

        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // managed objects
            }
            // unmanaged objects and resources
        }*/
    }
}