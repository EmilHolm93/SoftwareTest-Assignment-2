using System;
using MobileChargingStation.Lib.Models;

namespace MobileChargingStation.Lib.Interfaces
{
    public interface IDoor
    {
        event EventHandler<DoorChangedEventArgs> DoorChanged;

        bool IsOpen { get; }
        bool IsLocked { get; }

        void LockDoor();
        void UnlockDoor();
        void OpenDoor();
        void CloseDoor();
    }
}