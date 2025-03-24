using NUnit.Framework;
using MobileChargingStation.Lib.Interfaces;
using MobileChargingStation.Lib.Models;
using NSubstitute;
using System.IO;

namespace MobileChargingStation.Lib.Tests
{
    [TestFixture]
    public class StationControlTests
    {
        private StationControl _uut;
        private IDisplay _display;
        private IDoor _door;
        private IChargeControl _chargeControl;
        private IRFIDReader _rfidReader;

        private StringWriter _logOutput;
        //private string _logFile = "logfile.txt";

        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();
            _chargeControl = Substitute.For<IChargeControl>();
            _rfidReader = Substitute.For<IRFIDReader>();

            _uut = new StationControl(_rfidReader, _door, _chargeControl, _display);

            _logOutput = new StringWriter();
            Console.SetOut(_logOutput);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(Console.Out); // Gendan original Console Output
            _logOutput.Dispose(); // Frigiv ressourcer
        }

        [Test]
        public void RFIDDetected_WhenAvailable_AndPhoneConnected_ShouldLockAndStartCharge()
        {
            // Arrange - RFID detected with connected phone
            _chargeControl.Connected.Returns(true);
            int testId = 123;

            // Act - Raise event
            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(testId));

            //Assert - Check that the door is locked, charging is started and the display shows the correct message
            _door.Received(1).LockDoor();
            _chargeControl.Received(1).StartCharge();
            _display.Received().ShowMessage(Arg.Is<string>(msg => msg.Contains("Skabet er l")));
            
        }

        [Test]
        public void RFIDDetected_WhenAvailable_AndPhoneNotConnected_ShouldShowError()
        {
            _chargeControl.Connected.Returns(false);
            int testId = 123;

            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(testId));

            _display.Received().ShowError(Arg.Is<string>(msg => msg.Contains("ikke ordentlig tilsluttet")));
        }

        [Test]
        public void RFIDDetected_WhenLocked_WithCorrectRFID_ShouldUnlockAndStopCharge()
        {
            int testId = 123;
            _chargeControl.Connected.Returns(true);

            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(testId));
            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(testId));

            _chargeControl.Received(1).StopCharge();
            _door.Received(1).UnlockDoor();
            _display.Received().ShowMessage(Arg.Is<string>(msg => msg.Contains("Tag din telefon ud")));
        }

        [Test]
        public void RFIDDetected_WhenLocked_WithIncorrectRFID_ShouldShowError()
        {
            int testId = 123;
            int wrongId = 999;
            _chargeControl.Connected.Returns(true);

            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(testId));
            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(wrongId));

            _display.Received().ShowError(Arg.Is<string>(msg => msg.Contains("Forkert RFID tag")));
        }

        [Test]
        public void RFIDDetected_WhenDoorOpen_ShouldBeIgnored()
        {
            // Arrange: Simuler at døren er åben
            _door.DoorChanged += Raise.EventWith(new DoorChangedEventArgs { IsOpen = true });
            

            // Act: Prøv at scanne et RFID-tag
            _rfidReader.RFIDDetectedEvent += Raise.EventWith(new RFIDEventArgs(789));

            // Assert: Ingen handlinger skal udføres
            _door.DidNotReceive().LockDoor();
            _chargeControl.DidNotReceive().StartCharge();
            
        }

        [Test]
        public void DoorChanged_WhenOpened_ShouldShowMessage()
        {
            _door.DoorChanged += Raise.EventWith(new DoorChangedEventArgs { IsOpen = true });

            _display.Received().ShowMessage(Arg.Is<string>(msg => msg.Contains("ren er ")));
        }

        [Test]
        public void DoorChanged_WhenClosed_ShouldShowMessage()
        {
            _door.DoorChanged += Raise.EventWith(new DoorChangedEventArgs { IsOpen = true });
            _door.DoorChanged += Raise.EventWith(new DoorChangedEventArgs { IsOpen = false });

            _display.Received().ShowMessage(Arg.Is<string>(msg => msg.Contains("ren er lukket")));
        }


    }
}