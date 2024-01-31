using System;

namespace Scripts.Data
{
    [Flags]
    public enum AttackType
    {
        None = 0,
        Slash = 1 << 0, // 타격
        Penetrate= 1 << 1, // 관통
        Smash= 1 << 2, // 참격
        Wide= 1 << 3, // 광역
        Flame= 1 << 4, // 화염
        Freezing= 1 << 5, // 빙결
        Wind= 1 << 6, // 바람
        Lightning = 1 << 7, // 전격
        Light= 1 << 8, // 빛
        Dark= 1 << 9, // 어둠
        
        All = int.MaxValue
    }
}