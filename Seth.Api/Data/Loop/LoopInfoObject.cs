namespace Seth.Api.Data.Loop
{
    public enum LoopTimerDirection
    {
        Forward,
        Backward
    }

    public class LoopInfoObject
    {
        public double TimerMultiply = 1.0;
#pragma warning disable CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Name' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable property 'TaskId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string TaskId { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'TaskId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        public int MinUpdateTimer { get; set; }

        public int MaxUpdateTimer { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsPaused { get; set; } = false;

        public double CurrentMills { get; set; } = 0;

        public long TargetMills { get; set; } = 0;

        public long UpdateEveryMills { get; set; } = 0;

#pragma warning disable CS8618 // Non-nullable property 'Callback' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Action Callback { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Callback' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable property 'UpdateCallback' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Action UpdateCallback { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'UpdateCallback' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        public LoopTimerDirection Direction { get; set; }
    }
}