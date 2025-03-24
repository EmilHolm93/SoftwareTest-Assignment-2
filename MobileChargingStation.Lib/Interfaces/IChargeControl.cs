namespace MobileChargingStation.Lib.Interfaces;

public interface IChargeControl
{
    event EventHandler<string>? ChargeMessageEvent;
    bool Connected { get; }
    void StartCharge();
    void StopCharge();
}
