using MobileChargingStation.Lib.Interfaces;
using MobileChargingStation.Lib.Models;

namespace MobileChargingStation
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum MobileChargingStationState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Member variabler til at gemme tilstanden af systemet
        private MobileChargingStationState _state;
        private IChargeControl _charger;
        private int _oldId;
        private IDoor _door;
        private IRFIDReader _rfidReader;
        private IDisplay _display;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Constructor
        public StationControl(IRFIDReader rfidReader, IDoor door, IChargeControl charger, IDisplay display)
        {
            _state = MobileChargingStationState.Available;
            _rfidReader = rfidReader;
            _charger = charger;
            _door = door;
            _display = display;

            rfidReader.RFIDDetectedEvent += HandleRfidDetected;
            door.DoorChanged += HandleDoorChanged;
            
        }

       

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case MobileChargingStationState.Available:
                    // Check for ladeforbindelse
                    if (_charger.Connected)
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                       
                        Log($"Skab låst med RFID: {id}");
                        

                        _display.ShowMessage("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = MobileChargingStationState.Locked;
                    }
                    else
                    {
                        _display.ShowError("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case MobileChargingStationState.DoorOpen:
                    // Ignore
                    break;

                case MobileChargingStationState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                       
                        Log($"Skab låst op med RFID: {id}");

                        _display.ShowMessage("Tag din telefon ud af skabet og luk døren");
                        _state = MobileChargingStationState.Available;
                    }
                    else
                    {
                        _display.ShowError("Forkert RFID tag");
                    }

                    break;
            }
        }

        // Her mangler de andre trigger handlere

        private void HandleRfidDetected(object? sender, RFIDEventArgs e)
        {
            RfidDetected(e.Id);
        }

        private void HandleDoorChanged(object? sender, DoorChangedEventArgs e)
        {
            if (e.IsOpen)
            {
                if (_state == MobileChargingStationState.Available)
                {
                    _state = MobileChargingStationState.DoorOpen;
                    _display.ShowMessage("Døren er åben. Indsæt telefon og luk døren.");
                }
            }
            else // Handles the door closing
            {
                if (_state == MobileChargingStationState.DoorOpen)
                {
                    _state = MobileChargingStationState.Available;
                    _display.ShowMessage("Døren er lukket.");
                }
            }
        }

        private void Log(string msg)
        {
            using (var writer = File.AppendText(logFile))
            {
                writer.WriteLine($"{DateTime.Now}: {msg}");
            }
        }
    }
}
