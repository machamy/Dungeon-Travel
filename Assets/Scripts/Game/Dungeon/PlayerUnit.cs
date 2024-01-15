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
    void FixedUpdate()
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

    // Input System에서 사용. 패드,키보드 등의 입력을 vector2로 받아온다.
    void OnMove(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>(); //이미 normalized된 녀석.
        moveVec = new Vector3(inputVec.x, 0, inputVec.y);
    }
}
