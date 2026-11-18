namespace ToolShed;

/// <summary>Źródło „dzisiejszej” daty. W testach podstawiamy stałą datę.</summary>
public interface IClock
{
    DateOnly Today { get; }
}

public class SystemClock : IClock
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}

public class FixedClock : IClock
{
    public FixedClock(DateOnly today) => Today = today;

    public DateOnly Today { get; set; }
}
