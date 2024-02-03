using Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace Scripts.Game.Dungeon.Unit
{
    /*
    /// <summary>
    /// 아래 enum은 기획서에 따라 임의로 작명함. DB에 어떻게 저장되냐에 따라 이름 바꾸면 될듯.
    /// </summary>
    public enum MonsterAttackType
    {
        firstStrike = 0,    // 선제공격하는 경우
        neutral,            // 선제공격 안하는 경우 - 중립
        fleeing,            // 선제공격 안하는 경우 - 도망
        mustCombat          // 보스와 같이 강제 전투 걸리는 경우
    }

    public enum MonsterMoveType
    {
        doPatrol = 0,       // IDLE 상태에서 이리저리 움직이는지
        notPatrol           // IDLE 상태에서 자리가 고정되어있는지
    }

    public enum MonsterStates
    {
        Idle = 0,
        Chase,
        Flee,
        Return,
        Combat,
        Dead
    }

    /// <summary>
    /// 몬스터 객체 구현 스크립트. 직렬화된 변수들은 DB에 연결하기 전에 빠른 값 교체 용도로 사용함.
    /// </summary>
    public class MonsterUnit : AIBaseEntity
    {
        [SerializeField] Transform target;
        [SerializeField] string monsterName;
        [SerializeField] float defaultSpeed; // 패트롤 기능을 추가 시 사용할 값. 현재 기능 없음.
        [SerializeField] float chaseOrFleeSpeed;
        [SerializeField] float detectRadius;
        [SerializeField] float moveRadius;
        [SerializeField] float attackRange;
        [SerializeField] MonsterAttackType attackType;
        [SerializeField] MonsterMoveType moveType;

        Vector3 initialPosition; // 몬스터의 초기 위치. moveRadius의 결정 기준.

        bool canChase = false;
        bool isChasing = true;
        bool isAttacking = false;
        bool isFleeing = false;
        bool isDead = false;
        bool isPatroling = false;
        NavMeshAgent nav;
        Rigidbody rigid;

        private State<MonsterUnit>[] states;
        private StateMachine<MonsterUnit> stateMachine;

        #region PROPERTIES
        public Transform Target{ set => target = value; get => target; }
        public NavMeshAgent Nav { get { return nav; } }
        public bool CanChaseOrFlee { set => canChase = value; get => canChase; } 
        public float DetectRadius { set => detectRadius = value; get => detectRadius; }
        public float MoveRadius { set => moveRadius = value; get => moveRadius; }
        public float AttackRange { set => attackRange = value; get => attackRange; }
        public float DefaultSpeed { set => defaultSpeed = value; get => defaultSpeed; }
        public float ChaseOrFleeSpeed { set => chaseOrFleeSpeed = value; get => chaseOrFleeSpeed; }
        public bool IsChasing { set => isChasing = value; get => isChasing; }
        public bool IsFleeing { set => isFleeing = value; get => isFleeing; }


        public MonsterAttackType AttackType { get => attackType; }
        public MonsterMoveType MoveType { get => moveType; }
        public Vector3 InitialPosition { get => initialPosition; }

        #endregion


        private EnemyStatData mData;

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();           
        }

        public override void Setup(string name)
        {
            #region SYMBOL INITIALIZATION 
            base.Setup(name);

            gameObject.name = $"{ID}_{name}";

            defaultSpeed = 6f;
            chaseOrFleeSpeed = 4f;
            detectRadius = 5f;
            moveRadius = 10f;
            attackRange = 2f;

            attackType = MonsterAttackType.firstStrike;
            moveType = MonsterMoveType.doPatrol;

            initialPosition = transform.localPosition;
            target = GameObject.FindWithTag("Player").transform;
            //gameObject.name = 
            #endregion

            #region STATE INITIALIZATION
            states = new State<MonsterUnit>[6];
            states[(int)MonsterStates.Idle] = new MonsterOwnedStates.Idle();
            states[(int)MonsterStates.Chase] = new MonsterOwnedStates.Chase();
            states[(int)MonsterStates.Flee] = new MonsterOwnedStates.Flee();
            states[(int)MonsterStates.Return] = new MonsterOwnedStates.Return();
            states[(int)MonsterStates.Combat] = new MonsterOwnedStates.Combat();
            states[(int)MonsterStates.Dead] = new MonsterOwnedStates.Dead();

            stateMachine = new StateMachine<MonsterUnit>();

            stateMachine.Setup(this, states[(int)MonsterStates.Idle]);
            #endregion



        }

        public override void Updated()
        {
            stateMachine.Execute();
        }

        public void ChangeState(MonsterStates newState)
        {
            stateMachine.ChangeState(states[(int)newState]);
        }
       

        private void FixedUpdate()
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }

        
        /// <summary>
        /// Idle 상태일 때 몬스터의 정찰 행동. 원형범위 내 랜덤히 움직임.
        /// </summary>
        public void Patrol()
        {
            if (!isPatroling)
            {
                float patrolRadius = moveRadius - detectRadius;
                float patrolAngle = Random.Range(0, 360f);
                Vector3 nextPatrolPoint = initialPosition + new Vector3(patrolRadius * Random.Range(0f, 1f) * Mathf.Cos(Mathf.Deg2Rad * patrolAngle), 0,
                                                                        patrolRadius * Random.Range(0f, 1f) * Mathf.Sin(Mathf.Deg2Rad * patrolAngle));
                // 이동 가능한지 검사
                if(NavMesh.SamplePosition(nextPatrolPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    nav.SetDestination(nextPatrolPoint);
                    isPatroling = true;
                }                            
            }
            else
            {
                if(!nav.pathPending && nav.remainingDistance < 0.01f)
                {
                    isPatroling = false;
                }    
            }
        }


        private void OnDrawGizmos()
        {
            // 기즈모 1 - 검출 반경 (녹색)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);

            // 기즈모 2 - 이동 반경 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(initialPosition, moveRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
    */

    public class MonsterUnit : AIBaseEntity
    {
        #region INITIALIZATION

        [SerializeField] string monsterName;
        
        EnemyStatData monsterData;

        EnemyProperty monsterProperty;
        public EnemyProperty MonsterProperty { get { return monsterProperty; } }

        private NavMeshAgent nav;
        public NavMeshAgent Nav { get { return nav; } }

        private Vector3 initialPosition;
        public Vector3 InitialPosition { get { return initialPosition; } }

        public Transform target;

        public MeshRenderer meshRenderer;

        public Collider mosnterCollider;
        #endregion

        public override void Setup()
        {
            base.Setup();
            monsterData = DB.GetEnemyData(monsterName);
            gameObject.name = $"{ID}_{monsterName}";
            monsterProperty = monsterData.Property;

            nav = GetComponent<NavMeshAgent>();
            initialPosition = transform.localPosition;
            target = GameObject.FindWithTag("Player").transform;

            meshRenderer = GetComponent<MeshRenderer>();
            mosnterCollider = GetComponent<Collider>();
        }


        public override void Updated()
        {
            
        }
    }

}


/// <summary>
/// 몬스터 기준 적 감지하는 함수.
/// </summary>
/*void Detect()
{
    if (Vector3.Distance(transform.position, target.position) <= detectRadius)
    {
        canChase = true;
    }

}*/
    
/// <summary>
/// 몬스터 기준 적 쫓는 함수
/// navMesh 사용. 바닥을 static으로 설정해야 함.
/// </summary>
/*void Chase()
{
    if (!canChase) return;
    else
    {
        if (Vector3.Distance(transform.position, initialPosition) >= moveRadius)
        {
            canChase = false;

            ReturnToInit();
            return;
        }
        else if (Vector3.Distance(target.position, initialPosition) < moveRadius)
        {
            isChasing = true;
            nav.SetDestination(target.position);
            nav.speed = chaseSpeed;
            Debug.Log("추격 중!");
        }
    }
}

void ReturnToInit()
{
    if (isChasing && !canChase)
    {
        nav.SetDestination(initialPosition);
        nav.speed = defaultSpeed;
        Debug.Log("놓쳤다. 돌아가야지~");
    }
}

void Combat(bool isAttacking)
{
    if (isAttacking)
    {
        //몬스터가 선제공격한 상황으로 전투 씬 전개 필요
        Debug.Log("몬스터 선제공격!");
    }
    else
    {
        //플레이어가 선제공격한 상황으로 전투 씬 전개 필요
        Debug.Log("플레이어 선제공격!");
    }

    //전투 씬으로 넘기기
}

/// <summary>
/// 전투 이후 몬스터 죽을 때. 대체 가능할 것으로 보임.
/// </summary>
void MonsterDie()
{
    Destroy(gameObject);
}


/// <summary>
/// 선제공격 여부 판정. 플레이어 관련 콜라이더만 검출해서 전투 씬으로 넘기도록 한다.
/// </summary>
private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.tag == "Weapon")
    {
        isAttacking = false;
    }
    else if (collision.gameObject.tag == "Player")
    {
        isAttacking = true;
    }
    else return;

    Combat(isAttacking);

}*/