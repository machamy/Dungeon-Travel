using Scripts.Game.Dungeon;
using Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuraterviewCamera : AbstractCamera
{
    public Vector3 offset;


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
    
}
