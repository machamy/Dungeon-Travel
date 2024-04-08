using DG.Tweening;
using Scripts.Game.Dungeon.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraObstacleChecker : MonoBehaviour
{
    public LayerMask layer;
    [FormerlySerializedAs("target")] public GameObject Target;

    public HashSet<GameObject> HandlingObjects { get; private set; }

    [Header("x : 좌우 y : 위아래 z : 앞뒤")]
    public List<Vector3> rayOffsets;

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
        Vector3 targetPos = Target.transform.position;
        // ShootRay(targetPos);

        foreach (var offset in rayOffsets)
        {
            ShootRay(Target.transform.TransformPoint(offset));
        }
    }

    /// <summary>
    /// 최적화를 위해 따로 선언. 배열의 크기만큼 레이캐스트함.
    /// </summary>
    private RaycastHit[] hits = new RaycastHit[10];
    
    private void ShootRay(GameObject target) => ShootRay(target.transform.position);
    
    private void ShootRay(Vector3 targetPos){
        var originPos = transform.position;
        Vector3 v = (targetPos - originPos);
        var size = Physics.RaycastNonAlloc(originPos, v.normalized, hits, v.magnitude, layer);
        Debug.DrawRay(originPos, v.normalized* v.magnitude,Color.cyan);
        for(int i = 0; i < size; i++)
        {
            RaycastHit hit = hits[i];
            GameObject obj = hit.collider.gameObject;

            if(obj.TryGetComponent(out TransparentObstacleUnit toUnit) || obj.transform.parent.TryGetComponent(out toUnit))
            {
                toUnit.DoTransparent();
            }
            
        }
    }
}
