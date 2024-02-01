using Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Dungeon.Unit
{
    public enum monsterType
    {
        general = 0,
        elite
    }

    [CreateAssetMenu(fileName = "MonsterData", order = int.MaxValue)]
    public class MonsterData : ScriptableObject
    {        
        [SerializeField]
        private string enemyName;
        public string EnemyName { get { return enemyName; } }

        /// <summary>
        /// 난입 시, 가능한 몬스터 선별을 위한 지표
        /// </summary>
        [SerializeField]
        private int groupNum;
        public int GroupNum { get { return groupNum; } }

        [SerializeField]
        private Vector3 spawnPoint;
        public Vector3 SpawnPoint { get { return spawnPoint; } }

        [SerializeField]
        private monsterType monsterType;
        public monsterType MonsterType { get {  return monsterType; } }

        /// <summary>
        /// flag 형식의 Property. 선공, 이동방식
        /// </summary>
        [SerializeField]
        private EnemyProperty enemyproperty;
        public EnemyProperty EnemyProperty { get { return enemyproperty; } }

    }
}

