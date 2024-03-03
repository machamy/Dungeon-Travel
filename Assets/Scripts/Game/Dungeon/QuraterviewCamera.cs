using Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuraterviewCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool isMain = false;


    private void Start()
    {
        if(isMain)
            GameManager.Instance.qCamera = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    /// <summary>
    /// 카메라 기준으로 월드에서의 이동 방향을 구하는 함수
    /// </summary>
    /// <param name="carmeraDirection">카메라 기준 방향/param>
    /// <returns>월드 기준 방향</returns>
    public Vector3 GetWorldDiretion(Vector3 carmeraDirection)
    {
        float angle;
        Vector3 axis;
        transform.rotation.ToAngleAxis(out angle,out axis);

        Vector3 result = Quaternion.AngleAxis(angle, axis) * carmeraDirection;
        result.y = 0;
        result.Normalize();
        return result;
    }
}
