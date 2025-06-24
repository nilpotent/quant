namespace SkyQuant.Models;

public readonly record struct Snapshot(string RawLine, int? B0, int? BQ0, int? BN0, int? A0, int? AQ0, int? AN0);
