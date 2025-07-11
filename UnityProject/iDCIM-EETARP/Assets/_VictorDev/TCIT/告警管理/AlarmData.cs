using System;
using UnityEngine;

namespace VictorDev.TCIT.AlarmModule
{
    /// 告警資料架構(暫用)
    [Serializable]
    public class AlarmData
    {
        public string dateTimeString;
        public DateTime alarmTime => DateTime.Parse(dateTimeString.Trim());
        
        public enum AlarmType
        {
            Warning,
            Critical
        }

        public enum AlarmSystem
        {
            ACMV,
            EE,
            FS,
            PS,
            WE
        }

        public enum Status
        {
            Unresolved,
            Pending,
            Progress,
            Done,
        }

        public AlarmType alarmType = AlarmType.Warning;
        public AlarmSystem alarmSystem = AlarmSystem.ACMV;
        public Status status = Status.Unresolved;
        public Transform alarmTargetModel;
        public string OriginDeviceName => RevitConfig.GetDeviceName(alarmTargetModel);

        [TextArea(1, 5)] public string note;
    }
}