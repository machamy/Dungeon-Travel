using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody rigid;
    Vector3 moveVec;

    void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();

        playerTurn();
    }

    void playerMove()
    {
        transform.position += moveVec * speed * Time.deltaTime;
    }

    void playerTurn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    // Input System���� ���. �е�,Ű���� ���� �Է��� vector2�� �޾ƿ´�.
    void OnMove(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>(); //�̹� normalized�� �༮.
        moveVec = new Vector3(inputVec.x, 0, inputVec.y);
    }
}
