using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileChargingStation.Lib.Interfaces;

namespace MobileChargingStation.Lib.Models
{
    public class RFIDReader : IRFIDReader
    {
        public event EventHandler<RFIDEventArgs>? RFIDDetectedEvent;


        public void SimulateRfidScan(int id)
        {
            OnRfidDetected(new RFIDEventArgs(id));
        }

        public virtual void OnRfidDetected(RFIDEventArgs e)
        {
            RFIDDetectedEvent?.Invoke(this, e);
        }
    }
}
