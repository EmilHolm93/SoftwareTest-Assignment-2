using NUnit.Framework;
using MobileChargingStation.Lib.Models;
using MobileChargingStation.Lib.Interfaces;
using System;

namespace MobileChargingStation.Lib.Tests
{
    [TestFixture]
    public class DoorTests
    {
        private Door _uut;
        private DoorChangedEventArgs _receivedEventArgs;
        private bool _eventTriggered;

        [SetUp]
        public void Setup()
        {
            _uut = new Door();
            _eventTriggered = false;
            _receivedEventArgs = null;

            // Listening for DoorChangeEvent
            _uut.DoorChanged += (sender, args) =>
            {
                _eventTriggered = true;
                _receivedEventArgs = args;
            };
        }

        [Test]
        public void Door_StartsClosedAndUnlocked()
        {
            // Act: Ingen handling nødvendig, da vi tester initialtilstand

            // Assert: Bekræfter at døren starter lukket og ulåst
            Assert.That(_uut.IsOpen, Is.False);
            Assert.That(_uut.IsLocked, Is.False);
        }

        [Test]
        public void OpenDoor_WhenUnlocked_DoorOpens()
        {
            // Act: Åbn døren
            _uut.OpenDoor();

            // Assert: Døren skal være åben, og eventet skal være udløst
            Assert.That(_uut.IsOpen, Is.True);
            Assert.That(_eventTriggered, Is.True);
            Assert.That(_receivedEventArgs.IsOpen, Is.True);
        }

        [Test]
        public void OpenDoor_WhenLocked_DoorDoesNotOpen()
        {
            // Arrange: Lås døren
            _uut.LockDoor();
            _eventTriggered = false;

            // Act: Prøv at åbne døren
            _uut.OpenDoor();

            // Assert: Døren må ikke åbne, og intet event skal udløses
            Assert.That(_uut.IsOpen, Is.False);
            Assert.That(_eventTriggered, Is.False); // Now checking only for OpenDoor
        }

        [Test]
        public void CloseDoor_WhenOpen_DoorCloses()
        {
            // Arrange: Åbn døren først
            _uut.OpenDoor();
            _eventTriggered = false;

            // Act: Luk døren
            _uut.CloseDoor();

            // Assert: Døren skal være lukket, og eventet skal være udløst
            Assert.That(_uut.IsOpen, Is.False);
            Assert.That(_eventTriggered, Is.True); // Checking only for CloseDoor
            Assert.That(_receivedEventArgs.IsOpen, Is.False);
        }

        [Test]
        public void CloseDoor_WhenAlreadyClosed_NoEventTriggered()
        {
            // Act: Luk døren (som allerede er lukket)
            _uut.CloseDoor();

            // Assert: Døren skal forblive lukket, og intet event skal udløses
            Assert.That(_uut.IsOpen, Is.False);
            Assert.That(_eventTriggered, Is.False);
        }

        [Test]
        public void OpenDoor_WhenAlreadyOpen_NoEventTriggered()
        {
            // Arrange: Åbn døren først
            _uut.OpenDoor();
            _eventTriggered = false;

            // Act: Prøv at åbne den igen
            _uut.OpenDoor();

            // Assert: Døren forbliver åben, og intet event udløses
            Assert.That(_uut.IsOpen, Is.True);
            Assert.That(_eventTriggered, Is.False);
        }

        [Test]
        public void LockDoor_WhenClosed_DoorLocks()
        {
            // Act: Lås døren
            _uut.LockDoor();

            // Assert: Døren skal være låst, og eventet skal være udløst
            Assert.That(_uut.IsLocked, Is.True);
            Assert.That(_eventTriggered, Is.True);
            Assert.That(_receivedEventArgs.IsLocked, Is.True);
        }

        [Test]
        public void LockDoor_WhenOpen_DoorDoesNotLock()
        {
            // Arrange: Åbn døren først
            _uut.OpenDoor();
            _eventTriggered = false;

            // Act: Prøv at låse døren
            _uut.LockDoor();

            // Assert: Døren må ikke låses, og intet event udløses
            Assert.That(_uut.IsLocked, Is.False);
            Assert.That(_eventTriggered, Is.False);
        }

        [Test]
        public void UnlockDoor_WhenLocked_DoorUnlocks()
        {
            // Arrange: Lås døren først
            _uut.LockDoor();

            // Act: Lås døren op
            _uut.UnlockDoor();

            // Assert: Døren skal være ulåst, og eventet skal være udløst
            Assert.That(_uut.IsLocked, Is.False);
            Assert.That(_eventTriggered, Is.True);
            Assert.That(_receivedEventArgs.IsLocked, Is.False);
        }

        [Test]
        public void UnlockDoor_WhenAlreadyUnlocked_NoEventTriggered()
        {
            // Act: Lås op (døren er allerede ulåst)
            _uut.UnlockDoor();

            // Assert: Døren forbliver ulåst, og intet event udløses
            Assert.That(_uut.IsLocked, Is.False);
            Assert.That(_eventTriggered, Is.False);
        }
    }
}
