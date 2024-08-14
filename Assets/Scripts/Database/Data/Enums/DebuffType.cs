using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Data
{

    [Flags]
    public enum DebuffType
    {
        None = 0,       // 없음
        Blood = 1 << 0,     // 출혈
        Poison = 1 << 1,      // 중독
        Blind = 1 << 2,       // 실명
        Stun = 1 << 3,       // 기절
        Silence = 1 << 4,   // 침묵
        Confuse = 1 << 5,   // 혼란
        LastIdx = 6,
    }

    public static class DebuffTypeHelper
    {
        public static Dictionary<DebuffType, string> typeChange = new Dictionary<DebuffType, string>()
        {
            { DebuffType.Blood,"출혈"},
            {DebuffType.Poison , "중독" },
            {DebuffType.Blind , "실명" },   
            {DebuffType.Stun , "기절" },
            {DebuffType.Silence , "침묵"},
            {DebuffType.Confuse , "혼란"},
        };

        public static DebuffType GetFromKorean(string kor)
        {
            if (kor == "빛")
                kor = "광휘";
            var result = typeChange.FirstOrDefault(e => (e.Value == kor)).Key;
            return result;
        }
    }
}
