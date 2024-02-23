using Scripts.Game.Dungeon.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoubleRotationDoor : BaseInteractionUnit, IDoor
{
    public GameObject Left;
    public GameObject Right;
    public GameObject[] Lock;

    public float rotateAngle = 90f;
    public float rotateTime = 1f;
    public bool isOpened = false;
    private bool isMoving = false;
    public bool isLocked;

    private void Awake()
    {
        
    }

    public void Start()
    {
        if(isOpened)
            OpenFront();
    }

    
    public override void OnUsed(PlayerUnit unit)
    {
        float angle = Mathf.Abs(Vector3.Angle(transform.right, unit.transform.forward));
        if (isMoving)
            return;
        
        if (isOpened)
        {
            Close();
            return;
        }
        if (Mathf.Abs(angle) < 90f)
        {
            OpenFront();
        }
        else
        {
            OpenBackward();
        }
    }


    public void OpenFront()
    {
        
        isOpened = true;
        MoveDoor(rotateAngle);
    }

    public void OpenBackward()
    {
        isOpened = true;
        MoveDoor(-rotateAngle);
    }

    public void Open()
    {
        OpenFront();
    }

    private void MoveDoor(float targetAngle)
    {
        if (isMoving)
            return;
        StartCoroutine(MoveRoutine(targetAngle));
    }

    WaitForFixedUpdate waitForFixed = new WaitForFixedUpdate();
    private IEnumerator MoveRoutine(float targetAngle)
    {
        Quaternion RightOrigin = Right.transform.localRotation;
        Quaternion RightTarget = Quaternion.Euler(RightOrigin.x,targetAngle,RightOrigin.z);
        
        Quaternion LeftOrigin = Left.transform.localRotation;
        Quaternion LeftTarget = Quaternion.Euler(LeftOrigin.x,-targetAngle,LeftOrigin.z);
        
        isMoving = true;
        
        while (targetAngle < 0)
            targetAngle += 360;
        while (targetAngle > 360)
            targetAngle -= 360;

        float startTime = Time.time;
        float spentTime = Time.time - startTime;
        while (spentTime < rotateTime)
        {
            yield return waitForFixed;
            Right.transform.localRotation = Quaternion.Slerp(RightOrigin,RightTarget,spentTime/rotateTime);
            Left.transform.localRotation = Quaternion.Slerp(LeftOrigin,LeftTarget,spentTime/rotateTime);
            spentTime = Time.time - startTime;
        }

        Right.transform.localRotation = RightTarget;
        Left.transform.localRotation = LeftTarget;
        
        isMoving = false;
    }
    
    public void Close()
    {
        if (!isOpened)
            return;
        MoveDoor(0f);
        isOpened = false;
    }

    public override void OnFocusEvent(bool val)
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(Left);
        objs.Add(Right);
        objs.AddRange(Lock);
        foreach (var obj in objs)
        {
            FocusUnit fu = obj.GetComponent<FocusUnit>();
            if(fu)
                fu.OnFocusEvent(val);
        }
    }
}
