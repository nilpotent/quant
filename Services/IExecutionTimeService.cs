using SkyQuant.Models;

namespace SkyQuant.Services;

public interface IExecutionTimeService
{
    IReadOnlyList<Snapshot>? ExecutionTime(IReadOnlyList<Tick> ticks);
}