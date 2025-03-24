using NUnit.Framework;
using Moq;
using MobileChargingStation.Lib.Interfaces;
using MobileChargingStation.Lib.Models;
using UsbSimulator;

namespace MobileChargingStation.Lib.Tests
{
    [TestFixture]
    public class ChargeControlTests
    {
        private ChargeControl _uut;
        private Mock<IUsbCharger> _usbChargerMock;
        private Mock<IDisplay> _displayMock;

        [SetUp]
        public void Setup()
        {
            // Setup mocks
            _usbChargerMock = new Mock<IUsbCharger>();
            _displayMock = new Mock<IDisplay>();

            // Create an instance of ChargeControl with the mocks
            _uut = new ChargeControl(_usbChargerMock.Object, _displayMock.Object);
        }

        [Test]
        public void StartCharge_WithConnectedTrue_ShouldStartCharge()
        {
            // Arrange
            _usbChargerMock.Setup(c => c.Connected).Returns(true);

            // Act
            _uut.StartCharge();

            // Assert
            _usbChargerMock.Verify(c => c.StartCharge(), Times.Once);
            _displayMock.Verify(d => d.ShowMessage("Opladning startet."), Times.Once);
        }

        [Test]
        public void StartCharge_WithConnectedFalse_ShouldShowError()
        {
            // Arrange
            _usbChargerMock.Setup(c => c.Connected).Returns(false);

            // Act
            _uut.StartCharge();

            // Assert
            _displayMock.Verify(d => d.ShowError("Der er ingen forbindelse til telefon."), Times.Once);
        }

        [Test]
        public void StopCharge_ShouldStopCharge()
        {
            // Act
            _uut.StopCharge();

            // Assert
            _usbChargerMock.Verify(c => c.StopCharge(), Times.Once);
            
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void HandleCurrentChanged_LowCurrent_ShouldStopCharge(double current)
        {
            // Arrange
            var args = new CurrentEventArgs { Current = current };

            // Act
            _usbChargerMock.Raise(c => c.CurrentValueEvent += null, args);

            // Assert
            if (current > 0)
            {
                _displayMock.Verify(d => d.ShowMessage("Opladning tilendebragt. Telefon fuldt opladet."), Times.Once);
            }
        }

        [Test]
        public void HandleCurrentChanged_NormalCharging_ShouldDisplayChargingMessage()
        {
            // Arrange
            var args = new CurrentEventArgs { Current = 10 }; // Normal charging current

            // Act
            _usbChargerMock.Raise(c => c.CurrentValueEvent += null, args);

            // Assert
            _displayMock.Verify(d => d.ShowMessage("Oplader."), Times.Once);
        }

        [Test]
        public void HandleCurrentChanged_Overload_ShouldStopAndShowError()
        {
            // Arrange
            var args = new CurrentEventArgs { Current = 501 }; // Overload current

            // Act
            _usbChargerMock.Raise(c => c.CurrentValueEvent += null, args);

            // Assert
            _displayMock.Verify(d => d.ShowError("Fejl: Potentiel kortslutning. Opladning afsluttet."), Times.Once);
        }

        [Test]
        public void ChargeControl_Connected_ShouldReturnUsbChargerConnected()
        {
            // Arrange
            _usbChargerMock.Setup(c => c.Connected).Returns(true);
            // Act
            var connected = _uut.Connected;
            // Assert
            Assert.That(connected, Is.True);
        }

        [Test]
        public void ChargeControl_NotConnected_ShouldReturnUsbChargerNotConnected()
        {
            // Arrange
            _usbChargerMock.Setup(c => c.Connected).Returns(false);
            // Act
            var connected = _uut.Connected;
            // Assert
            Assert.That(connected, Is.False);
        }
    }
}