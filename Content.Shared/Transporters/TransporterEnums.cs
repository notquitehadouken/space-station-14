namespace Content.Shared.Transporters;

public enum TransporterVisualLayers : byte
{
    Light
}

public enum TransporterStatus : byte
{
    NoBattery,
    NoCharge,
    LowCharge,
    Charging,
    Waiting,
    Moving
}
