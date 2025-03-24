using System;
using System.Threading;
using MobileChargingStation.Lib.Interfaces;
using UsbSimulator;

namespace MobileChargingStation
{
    public class ChargeControl : IChargeControl
    {
        private readonly IUsbCharger _usbCharger;
        private readonly IDisplay _display;

        public event EventHandler<string>? ChargeMessageEvent;

        public bool Connected => _usbCharger.Connected;

        public ChargeControl(IUsbCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            _usbCharger.CurrentValueEvent += HandleCurrentChanged;
        }

        public void StartCharge()
        {
            if (Connected)
            {
                _usbCharger.StartCharge();
                _display.ShowMessage("Opladning startet.");
                ChargeMessageEvent?.Invoke(this, "Opladning startet.");
            }
            else
            {
                _display.ShowError("Der er ingen forbindelse til telefon.");
                ChargeMessageEvent?.Invoke(this, "Fejl: Der er ingen forbindelse til telefon.");
            }
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
            ChargeMessageEvent?.Invoke(this, "Opladning afsluttet.");
        }

        private void HandleCurrentChanged(object sender, CurrentEventArgs e)
        {
            if (e.Current > 0 && e.Current <= 5)
            {
                StopCharge();
                _display.ShowMessage("Opladning tilendebragt. Telefon fuldt opladet.");
                ChargeMessageEvent?.Invoke(this, "Opladning tilendebragt. Telefon fuldt opladet.");
                Thread.Sleep(100);
            }
            else if (e.Current > 5 && e.Current <= 500)
            {
                _display.ShowMessage("Oplader.");
                ChargeMessageEvent?.Invoke(this, "Oplader.");
                Thread.Sleep(100);
            }
            else if (e.Current > 500)
            {
                StopCharge();
                _display.ShowError("Fejl: Potentiel kortslutning. Opladning afsluttet.");
                ChargeMessageEvent?.Invoke(this, "Fejl: Overbelastning registreret. Opladning stoppet.");
                Thread.Sleep(100);
            }
        }
    }
}
