using System;

namespace Scripts.Data
{
    
    [Flags]
    public enum TargetType
    {
        None = 0,       // 없음
        Single = 1 << 0,     // 단일
        Front = 1 << 1,      // 전열
        Back = 1 << 2,       // 전후
        Area = 1 << 3,       // 광역
        All = 1 << 10 - 1
    }
}