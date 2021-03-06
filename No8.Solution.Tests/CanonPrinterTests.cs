﻿using System;
using System.IO;
using No8.Solution.Interfaces.IPrinterImplementations.PrinterImplementations;
using NUnit.Framework;

namespace No8.Solution.Tests
{
    [TestFixture]
    class CanonPrinterTests
    {
        #region CanonPrinter.Constructors

        [TestCase("model1")]
        [TestCase("model1-1-1-1234")]
        [TestCase("model 1234")]
        public void CanonPrinter_ValidModel_NewInstanceOfCanonPrinter(string model)
        {
            var printer = new CanonPrinter(model);

            Assert.IsTrue(printer != null);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("         ")]
        public void CanonPrinter_InvalidModel_ExpectedArgumentException(string model)
            => Assert.Throws<ArgumentException>(() => new CanonPrinter(model));

        #endregion

        #region CanonPrinter.Properties
        [TestCase("model1")]
        [TestCase("model1-1-1-1234")]
        [TestCase("model 1234")]
        public void Name_GetName_ExpectedCanon(string model)
        {
            var printer = new CanonPrinter(model);

            StringAssert.AreEqualIgnoringCase("Canon", printer.Name);
        }

        [TestCase("model1")]
        [TestCase("model1-1-1-1234")]
        [TestCase("model 1234")]
        public void Model_GetModel_ExpectedModel(string model)
        {
            var printer = new CanonPrinter(model);

            StringAssert.AreEqualIgnoringCase(model, printer.Model);
        }

        #endregion

        #region CanonPrinter.PublicMethods

        [TestCase("model1", "model1", ExpectedResult = true)]
        [TestCase("model1-1-1-1234", "model1", ExpectedResult = false)]
        [TestCase("model 1234", "model1234", ExpectedResult = false)]
        public bool Equals_TwoPrinters_AreEqual(string model1, string model2)
        {
            var printer1 = new CanonPrinter(model1);
            var printer2 = new CanonPrinter(model2);

            return printer2.Equals(printer1);
        }

        [TestCase("model1")]
        [TestCase("model1-1-1-1234")]
        [TestCase("model 1234")]
        public void ToString_Printer_PrinterInStringFormat(string model)
        {
            var printer1 = new CanonPrinter(model);

            var expectedResult = "Canon - " + model;

            StringAssert.AreEqualIgnoringCase(expectedResult, printer1.ToString());
        }

        [TestCase("model1", null)]
        [TestCase("model1-1-1-1234", null)]
        [TestCase("model 1234", null)]
        public void Print_StreamIsNull_ExpectedArgumentNullException(string model, Stream stream)
            => Assert.Throws<ArgumentNullException>(() => new CanonPrinter(model).Print(stream));

        #endregion
    }
}