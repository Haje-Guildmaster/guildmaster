using System;

namespace GuildMaster.Tools
{
    public interface IReadOnlyChangeTrackedValue<out T>
    {
        event Action Changed;
        T Value { get; }
    }
}