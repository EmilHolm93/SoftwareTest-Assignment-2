using System;
using MobileChargingStation.Lib.Interfaces;

namespace MobileChargingStation.Lib.Models
{
    public class Door : IDoor
    {
        private bool _isOpen = false;
        private bool _isLocked = false;

        // Declare the event using EventHandler
        public event EventHandler<DoorChangedEventArgs> DoorChanged;

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;

        public void OpenDoor()
        {
            if (!_isOpen && !_isLocked) // Can only open if it's closed and not locked
            {
                _isOpen = true;
                OnDoorChanged();
            }
        }

        public void CloseDoor()
        {
            if (_isOpen) // Can only close if it's open
            {
                _isOpen = false;
                OnDoorChanged();
            }
        }

        public void LockDoor()
        {
            if (!_isLocked && !_isOpen) // Can only lock if it's closed and not already locked
            {
                _isLocked = true;
                OnDoorChanged();
            }
        }

        public void UnlockDoor()
        {
            if (_isLocked) // Can only unlock if it's locked
            {
                _isLocked = false;
                OnDoorChanged();
            }
        }

        // Method to invoke the DoorChanged event
        protected virtual void OnDoorChanged()
        {
            DoorChanged?.Invoke(this, new DoorChangedEventArgs { IsOpen = _isOpen, IsLocked = _isLocked });
        }
    }

    public class DoorChangedEventArgs : EventArgs
    {
        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
    }
}