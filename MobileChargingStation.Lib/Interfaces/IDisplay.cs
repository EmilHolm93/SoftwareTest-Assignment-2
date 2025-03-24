using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileChargingStation.Lib.Interfaces
{
    public interface IDisplay
    {
        void ShowMessage(string message);
        void ShowError(string errorMessage);
    }
}