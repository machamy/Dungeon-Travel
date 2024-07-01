using OpenCover.Framework.Model;
using System;
using Script.Data;
using Script.Global;
using Scripts.Entity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Data
{
    [CreateAssetMenu]
    [Serializable]
    public class SkillData : ScriptableObject, IDBdata
    {
        public ClassType classType;
        public string enemyName;
        
        public string weaponType;
        public int rank;
        public string rawType;

        public string skillName;
        public string infomation;

        public string parent;
        public bool isParent;
        public bool isChildUnlock;

        public float physicsDamage;
        public float propertyDamage;
        public float buffRatio;
        public float mpCost;
        public int skillWeight;

        public bool isPassive;
        public bool isSelf;

        public TargetType allyTargetType;
        public TargetType enemyTargetType;

        public bool isBuff;
        public bool isDebuff;
        public bool isHealing;
        public bool isRanged;
        public bool isMelee;
        
        public AttackType attackType;
        public DebuffType debuffType;

        public int pointCost = 1;

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(classType)}: {classType}, {nameof(enemyName)}: {enemyName}, {nameof(weaponType)}: {weaponType}, {nameof(rank)}: {rank}, {nameof(rawType)}: {rawType}, {nameof(skillName)}: {skillName}, {nameof(parent)}: {parent}, {nameof(isParent)}: {isParent}, {nameof(isChildUnlock)}: {isChildUnlock}, {nameof(physicsDamage)}: {physicsDamage}, {nameof(propertyDamage)}: {propertyDamage}, {nameof(mpCost)}: {mpCost}, {nameof(skillWeight)}: {skillWeight}, {nameof(isPassive)}: {isPassive}, {nameof(isSelf)}: {isSelf}, {nameof(allyTargetType)}: {allyTargetType}, {nameof(enemyTargetType)}: {enemyTargetType}, {nameof(isBuff)}: {isBuff}, {nameof(isDebuff)}: {isDebuff}, {nameof(isHealing)}: {isHealing}, {nameof(isRanged)}: {isRanged}, {nameof(isMelee)}: {isMelee}, {nameof(attackType)}: {attackType},{nameof(debuffType)}: {debuffType} ,{nameof(pointCost)}: {pointCost}";
        }
    }
}