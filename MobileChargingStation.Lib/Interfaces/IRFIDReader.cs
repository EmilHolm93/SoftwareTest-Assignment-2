using System;
namespace MobileChargingStation.Lib.Interfaces;

public interface IRFIDReader
{
    event EventHandler<RFIDEventArgs>? RFIDDetectedEvent;

    void SimulateRfidScan(int id);
}

public class RFIDEventArgs : EventArgs
{
    public int Id { get; }

    
    public RFIDEventArgs(int id)
    {
        Id = id;
    }
}