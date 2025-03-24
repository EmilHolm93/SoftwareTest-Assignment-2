using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileChargingStation.Lib.Interfaces;

namespace MobileChargingStation.Lib.Models
{
    public class Display : IDisplay
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine($"[INFO]: {message}");
        }

        public void ShowError(string errorMessage)
        {
            Console.WriteLine($"[ERROR]: {errorMessage}");
        }

    }
}