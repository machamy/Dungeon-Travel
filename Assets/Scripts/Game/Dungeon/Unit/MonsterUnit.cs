using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Game.Dungeon.Unit
{   
    /// <summary>
    /// ���� ��ü ���� ��ũ��Ʈ.
    /// ���� ��� ��� ��ȹ ����. �� ��ü AI�� ������ ���Ϳ��� ����� �� ���Ƽ�.
    /// </summary>
    public class MonsterUnit : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float defaultSpeed; // ��Ʈ�� ����� �߰� �� ����� ��. ���� ��� ����.
        [SerializeField] float chaseSpeed;
        [SerializeField] float detectRadius;
        [SerializeField] float moveRadius;

        Vector3 initialPosition; // ������ �ʱ� ��ġ. moveRadius�� ���� ����.
        bool canChase = false;
        bool isChasing = true;
        NavMeshAgent nav;
        Rigidbody rigid;

        bool isAttacking = false;

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            initialPosition = transform.localPosition;
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
        /// ���� ���� �� �����ϴ� �Լ�.
        /// </summary>
        void Detect()
        {
            if(Vector3.Distance(transform.position, target.position) <= detectRadius)
            {
                canChase = true;
            }

        }

        /// <summary>
        /// ���� ���� �� �Ѵ� �Լ�
        /// navMesh ���. �ٴ��� static���� �����ؾ� ��.
        /// </summary>
        void Chase()
        {
            if (!canChase) return;
            else
            {
                if (Vector3.Distance(transform.position, initialPosition) >= moveRadius )
                {
                    canChase = false;

                    ReturnToInit();
                    return;
                }
                else if(Vector3.Distance(target.position, initialPosition) < moveRadius)
                {
                    isChasing = true;
                    nav.SetDestination(target.position);
                    nav.speed = chaseSpeed;
                    Debug.Log("�߰� ��!");
                }               
            }
        }

        void ReturnToInit()
        {
            if(isChasing && !canChase)
            {
                nav.SetDestination(initialPosition);
                nav.speed = defaultSpeed;
                Debug.Log("���ƴ�. ���ư�����~");
            }
        }

        void Combat(bool isAttacking)
        {
            if(isAttacking)
            {
                //���Ͱ� ���������� ��Ȳ���� ���� �� ���� �ʿ�
                Debug.Log("���� ��������!");
            }
            else
            {
                //�÷��̾ ���������� ��Ȳ���� ���� �� ���� �ʿ�
                Debug.Log("�÷��̾� ��������!");
            }

            //���� ������ �ѱ��
        }

        /// <summary>
        /// ���� ���� ���� ���� ��. ��ü ������ ������ ����.
        /// </summary>
        void MonsterDie()
        {
            Destroy(gameObject);
        }


        /// <summary>
        /// �������� ���� ����. �÷��̾� ���� �ݶ��̴��� �����ؼ� ���� ������ �ѱ⵵�� �Ѵ�.
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
            // ����� 1 - ���� �ݰ� (���)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectRadius);

            // ����� 2 - �̵� �ݰ� (������)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(initialPosition, moveRadius);
        }
    }

}
