using System.Diagnostics.CodeAnalysis;
using MobileChargingStation;
using MobileChargingStation.Lib.Interfaces;
using MobileChargingStation.Lib.Models;
using UsbSimulator;


class Program
{
    static void Main(string[] args)
    {
        // Create the dependencies
        IDoor door = new Door();
        IRFIDReader rfidReader = new RFIDReader();
        IUsbCharger usbCharger = new UsbChargerSimulator(); // Assuming UsbChargerSimulator implements IUsbCharger
        IDisplay display = new Display(); // Assuming Display implements IDisplay
        IChargeControl charger = new ChargeControl(usbCharger, display);

        // Create the StationControl
        StationControl stationControl = new StationControl(rfidReader, door, charger, display);

        bool finish = false;
        do
        {
            string input;
            System.Console.WriteLine("Indtast E for Exit, O for Open, C Close, R for RFID: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            switch (input[0])
            {
                case 'E':
                    finish = true;
                    break;

                case 'O':
                    door.OpenDoor();
                    break;

                case 'C':
                    door.CloseDoor();
                    break;

                case 'R':
                    System.Console.WriteLine("Indtast RFID id: ");
                    string idString = System.Console.ReadLine();
                    if (!string.IsNullOrEmpty(idString))
                    {
                        int id = Convert.ToInt32(idString);
                        rfidReader.SimulateRfidScan(id);
                    }
                    else
                    {
                        display.ShowError("Ugyldigt RFID tag");
                    }

                    break;

                default:
                    break;
            }

        } while (!finish);
    }
}

