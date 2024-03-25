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
        public static Dictionary<string,DebuffType> typeChange = new Dictionary<string,DebuffType>()
        {
            {"출혈", DebuffType.Blood},
            {"중독" , DebuffType.Poison },
            {"실명" , DebuffType.Blind },   
            {"기절" , DebuffType.Stun },
            {"침묵" , DebuffType.Silence},
            {"혼란" , DebuffType.Confuse},
        };
    }
}
