using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Game.Dungeon.Unit
{
    /// <summary>
    /// 아래 enum은 기획서에 따라 임의로 작명함. DB에 어떻게 저장되냐에 따라 이름 바꾸면 될듯.
    /// </summary>
    public enum MonsterAttackType
    {
        firstStrike = 0,
        noFirstStrike,
        mustCombat
    }

    public enum MonsterMoveType
    {
        doPatrol = 0,
        notPatrol
    }



    /// <summary>
    /// 몬스터 객체 구현 스크립트.
    /// 아직 상속 사용 계획 없음. 적 객체 AI를 지금은 몬스터에만 사용할 것 같아서.
    /// </summary>
    public class MonsterUnit : AIBaseEntity
    {
        [SerializeField] Transform target;
        [SerializeField] float defaultSpeed; // 패트롤 기능을 추가 시 사용할 값. 현재 기능 없음.
        [SerializeField] float chaseSpeed;
        [SerializeField] float detectRadius;
        [SerializeField] float moveRadius;
        [SerializeField] MonsterAttackType attackType;
        [SerializeField] MonsterMoveType moveType;


        Vector3 initialPosition; // 몬스터의 초기 위치. moveRadius의 결정 기준.
        bool canChase = false;
        bool isChasing = true;
        NavMeshAgent nav;
        Rigidbody rigid;

        bool isAttacking = false;



        public override void Setup(string name)
        {
            base.Setup(name);

            gameObject.name = $"{ID}_{name}";

            defaultSpeed = 0;
            chaseSpeed = 0;
            detectRadius = 0;
            moveRadius = 0;

            attackType = 0;
            moveType = 0;

            initialPosition = transform.localPosition;
        }

        public override void Updated()
        {
            Debug.Log("대기중~");
        }

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Detect();

            Chase();
        }

        private void FixedUpdate()
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// 몬스터 기준 적 감지하는 함수.
        /// </summary>
        void Detect()
        {
            if (Vector3.Distance(transform.position, target.position) <= detectRadius)
            {
                canChase = true;
            }

        }

        /// <summary>
        /// 몬스터 기준 적 쫓는 함수
        /// navMesh 사용. 바닥을 static으로 설정해야 함.
        /// </summary>
        void Chase()
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

        }

        private void OnDrawGizmos()
        {
            // 기즈모 1 - 검출 반경 (녹색)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);

            // 기즈모 2 - 이동 반경 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(initialPosition, moveRadius);
        }
    }

}
