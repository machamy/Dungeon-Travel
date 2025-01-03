
using OpenCover.Framework.Model;
using System;
using Script.Data;
using Script.Global;
using Scripts.Entity;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Scripts.Data
{
    [CreateAssetMenu]
    [Serializable]
    public class SkillData : ScriptableObject, IDBdata
    {
        public ClassType classType;
        public string enemyName;
        
        public string weaponType;
        public int rank = 0; // 적의 스킬 랭크는 0으로 맞추기 위해서
        public string rawType;

        public string skillName;
        public string infomation;

        public string parent;
        public bool isParent;
        public bool isChildUnlock;

        // TODO : 임시 땜빵, 사용 금지
        public float physicsDamage => Random.Range(minPhysicsDamage.GetRaw(),maxPhysicsDamage.GetRaw());
        public float propertyDamage=> Random.Range(minPropertyDamage.GetRaw(),maxPropertyDamage.GetRaw());

        public SkillCalculateElement minPhysicsDamage;
        public SkillCalculateElement maxPhysicsDamage;
        public SkillCalculateElement minPropertyDamage;
        public SkillCalculateElement maxPropertyDamage;
        
        
        public float buffRatio;
        public float debuffRatio;
        public SkillCalculateElement mpCost;
        public int skillWeight;

        public bool isPassive;
        public bool isSelf;

        public TargetType targetType;
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
        public int skillLevel = 0;

        public override string ToString()
        {
            return
                $"{base.ToString()}, {nameof(classType)}: {classType}, {nameof(enemyName)}: {enemyName}, {nameof(weaponType)}: {weaponType}, {nameof(rank)}: {rank}, {nameof(rawType)}: {rawType}, {nameof(skillName)}: {skillName}, {nameof(infomation)}: {infomation}, {nameof(parent)}: {parent}, {nameof(isParent)}: {isParent}, {nameof(isChildUnlock)}: {isChildUnlock}, {nameof(minPhysicsDamage)}: {minPhysicsDamage}, {nameof(maxPhysicsDamage)}: {maxPhysicsDamage}, {nameof(minPropertyDamage)}: {minPropertyDamage}, {nameof(maxPropertyDamage)}: {maxPropertyDamage}, {nameof(buffRatio)}: {buffRatio}, {nameof(mpCost)}: {mpCost}, {nameof(skillWeight)}: {skillWeight}, {nameof(isPassive)}: {isPassive}, {nameof(isSelf)}: {isSelf}, {nameof(allyTargetType)}: {allyTargetType}, {nameof(enemyTargetType)}: {enemyTargetType}, {nameof(isBuff)}: {isBuff}, {nameof(isDebuff)}: {isDebuff}, {nameof(isHealing)}: {isHealing}, {nameof(isRanged)}: {isRanged}, {nameof(isMelee)}: {isMelee}, {nameof(attackType)}: {attackType}, {nameof(debuffType)}: {debuffType}, {nameof(pointCost)}: {pointCost}";
        }
    }
}