using System;

namespace Scripts.Data
{
    [Flags]
    public enum EnemyProperty
    {
        None = 0,
        Hostile = 1 << 0, // 선공 여부
        Movement= 1 << 1, // 이동 or 고정

        All = int.MaxValue
    }
}