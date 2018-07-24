using System;

namespace MFVolumeCtrl.Models.Script
{
    public enum ScheduleInterval
    {
        OnStart = 0,
        Daily = 1,
        Monthly = 2,
        Once = 3
    }
    [Serializable]
    public class ScheduleModel
    {
        public CommandModel Command { get; set; }
        public DateTime StartTime { get; set; }
        public ScheduleInterval Interval { get; set; }
    }
}
