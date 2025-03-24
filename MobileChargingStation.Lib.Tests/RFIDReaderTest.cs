using NUnit.Framework;
using System;
using MobileChargingStation.Lib.Interfaces;
using MobileChargingStation.Lib.Models;
using NSubstitute;

namespace MobileChargingStation.Lib.Tests
{
    [TestFixture]
    public class RFIDReaderTest
    {
        private IRFIDReader _uut;
        private RFIDEventArgs? _receivedEventArgs;


        [SetUp]
        public void Setup()
        {
            _uut = new RFIDReader();
            _receivedEventArgs = null;
        }

        [Test]
        public void SimulateRfidScan_EventIsTriggered_CorrectIdReceived()
        {
            // Arrange:  Opret eventhandler og lyt til RFIDDetectedEvent
            _uut.RFIDDetectedEvent += (sender, e) => _receivedEventArgs = e;

            // Act: Simuler en RFID-scanning med id = 123
            _uut.SimulateRfidScan(123);

            // Assert: Sikre, at eventen blev kaldt og at det rigtige id blev modtaget
            Assert.That(_receivedEventArgs, Is.Not.Null);
            Assert.That(_receivedEventArgs?.Id, Is.EqualTo(123));
        }

        [Test]
        public void SimulateRfidScan_EventIsTriggered_OnlyOnce()
        {
            int eventCount = 0;

            // Arrange: Opret eventhandler og optæl antal gange eventen bliver kaldt
            _uut.RFIDDetectedEvent += (sender, e) => eventCount++;

            // Act: Simuler en RFID-scanning
            _uut.SimulateRfidScan(456);

            // Assert: Sikre, at eventen kun blev kaldt én gang
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void SimulateRfidScan_NoEventListeners_NoExceptionThrown()
        {
            // Arrange: Ingen eventlisteners
            // Act & Assert: Sikre, at det ikke fejler, selvom der ikke er nogen event-subscribers
            Assert.DoesNotThrow((() => _uut.SimulateRfidScan(789)));
            
        }

    }
}

