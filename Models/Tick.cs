namespace SkyQuant.Models;

public readonly record struct Tick(long SourceTime, byte Side, string Action, ulong OrderId, int Price, int Qty, string RawLine);
