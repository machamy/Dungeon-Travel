using System;

namespace Scripts.Data
{
    [Flags]
    public enum EnemyProperty
    {
        None = 0,
        Hostile = 1 << 0, // 선공 여부
        Move= 1 << 1, // 이동 or 고정
        Chase = 1 << 2, // 추적 or 도망
        
        All = int.MaxValue
    }
}