using DG.Tweening;
using Scripts.Game.Dungeon.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleChecker : MonoBehaviour
{
    public LayerMask layer;
    public GameObject target;

    public HashSet<GameObject> HandlingObjects { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        HandlingObjects = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 v = (target.transform.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, v.normalized, v.magnitude, layer);
        Debug.DrawRay(transform.position, v.normalized* v.magnitude,Color.cyan);
        foreach (var hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            
            TransparentObstacleUnit toUnit;
            
            if(obj.TryGetComponent(out toUnit) || obj.transform.parent.TryGetComponent(out toUnit))
            {
                toUnit.DoTransparent();
            }
            
        }
    }
}
