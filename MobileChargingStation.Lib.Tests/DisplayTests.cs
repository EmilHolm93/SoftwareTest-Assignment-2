using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using MobileChargingStation.Lib.Models;
using MobileChargingStation.Lib.Interfaces;

namespace MobileChargingStation.Lib.Tests
{
    [TestFixture]
    public class DisplayTests
    {
        private Display _uut;
        private StringWriter _consoleOutput;
        private TextWriter _originalOutput;

        [SetUp]
        public void Setup()
        {
            // Gem den originale Console Output for at kunne gendanne den senere
            _originalOutput = Console.Out;

            _uut = new Display();
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            // Gendan original Console Output
            Console.SetOut(_originalOutput);

            // Dispose StringWriter for at frigive ressourcer
            _consoleOutput.Dispose();
        }

        [Test]
        public void ShowMessage_ShouldPrintCorrectMessage()
        {
            // Arrange
            string expectedOutput = "[INFO]: Test besked";

            // Act
            _uut.ShowMessage("Test besked");

            // Assert
            string output = _consoleOutput.ToString().Trim();
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void ShowError_ShouldPrintCorrectErrorMessage()
        {
            // Arrange
            string expectedOutput = "[ERROR]: Fejlbesked";

            // Act
            _uut.ShowError("Fejlbesked");

            // Assert
            string output = _consoleOutput.ToString().Trim();
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        // Edge Cases for at teste om metoderne håndterer null input (tom string)
        [Test]
        public void ShowMessage_ShouldPrintCorrectMessage_WhenMessageIsEmpty()
        {
            // Arrange
            string expectedOutput = "[INFO]: ";

            // Act
            _uut.ShowMessage("");

            // Assert
            string output = _consoleOutput.ToString();
            Assert.That(output, Is.EqualTo(expectedOutput + Environment.NewLine));
        }

        [Test]
        public void ShowError_ShouldPrintCorrectErrorMessage_WhenErrorMessageIsEmpty()
        {
            // Arrange
            string expectedOutput = "[ERROR]: ";
            // Act
            _uut.ShowError("");
            // Assert
            string output = _consoleOutput.ToString();
            Assert.That(output, Is.EqualTo(expectedOutput + Environment.NewLine));
        }

    }
}
