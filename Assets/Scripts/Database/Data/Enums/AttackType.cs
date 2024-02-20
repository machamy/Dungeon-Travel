using System;
using System.Collections.Generic;
using System.Linq;
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
        //TODO 작업중
        private static Dictionary<AttackType, string> eng2kor = new Dictionary<AttackType, string>()
        {
            {AttackType.Slash, "참격"},
            {AttackType.Penetrate, "관통"},
            {AttackType.Smash, "타격"},
            {AttackType.Flame, "화염"},
            {AttackType.Freezing, "빙결"},
            {AttackType.Wind, "바람"},
            {AttackType.Lightning,"빛"},
            {AttackType.Dark, "어둠"}
        };
        //
        // static string[] koreanNames = 
        //     { "참격", "관통", "타격", "화염", "빙결", "바람", "전격", "빛", "어둠" };
        //
        // static string[] englishNames =
        // { "Slash", "Penetrate", "Smash", "Flame", "Freezing", "Wind", "Lightning", "Light", "Dark" };

        public static string GetCodename(this AttackType type)
        {
            string result;
            if(eng2kor.TryGetValue(type, out result))
                return result;
            return "NLL";
        }
        public static AttackType GetFromKorean(string kor)
        {
            var result = eng2kor.FirstOrDefault(e => (e.Value == kor)).Key;
            return result;
        }
    }
    
}