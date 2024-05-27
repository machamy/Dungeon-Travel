using Scripts.Game.Dungeon;
using Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksideCamera : AbstractCamera
{
    public float distance;
    public float cameraSpeed;
    
    private void Start()
    {
        if(isMain)
            GameManager.Instance.qCamera = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 TodoPos = target.position + -target.transform.forward * distance + Vector3.up * 5; // 임시로 3위로 옮기기
        this.transform.position = Vector3.Lerp(transform.position,TodoPos,cameraSpeed * Time.deltaTime);
        transform.LookAt(target);
    }

    
}
