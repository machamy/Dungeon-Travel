using System;
using UnityEngine;

namespace Scripts.Data
{
    [Flags]
    public enum AttackType
    {
        None = 0,
        Slash = 1 << 0, // 참격
        Penetrate= 1 << 1, // 관통
        Smash= 1 << 2, // 타격
        Flame= 1 << 3, // 화염
        Freezing= 1 << 4, // 빙결
        Wind= 1 << 5, // 바람
        Lightning = 1 << 6, // 전격
        Light= 1 << 7, // 빛
        Dark= 1 << 8, // 어둠
        
        All = int.MaxValue
    }

    public static class AttackTypeHelper
    {
        static string[] koreanNames = 
            { "참격", "관통", "타격", "화염", "빙결", "바람", "전격", "빛", "어둠" };

        static string[] englishNames =
        { "Slash", "Penetrate", "Smash", "Flame", "Freezing", "Wind", "Lightning", "Light", "Dark" };

        public static AttackType GetFromKorean(string kor)
        {
            int idx = Array.IndexOf(koreanNames, kor);
            if (idx < 0)
                return AttackType.None;
            string eng = englishNames[idx];
            AttackType res;
            Enum.TryParse<AttackType>(eng, out res);
            return res;
        }

        public static string ToKorean(this AttackType type)
        {
            string kor = koreanNames[Array.IndexOf(englishNames, type.ToString())];
            return kor;
        }
    }
    
}