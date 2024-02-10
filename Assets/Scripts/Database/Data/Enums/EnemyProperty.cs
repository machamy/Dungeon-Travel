using System;

namespace Scripts.Data
{
    [Flags]
    public enum EnemyProperty
    {
        None = 0,
        Hostile = 1 << 0, // 선공 여부 : 선공 = 추적 / 비선공 = 배회
        Movement= 1 << 1, // 이동 or 고정

        All = int.MaxValue
    }
}