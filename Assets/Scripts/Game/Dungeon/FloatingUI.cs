using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatingUI : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    private RectTransform _rectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();
        // _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            MovePos();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void MovePos()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.gameObject.transform.position);
        transform.position = screenPos;
    }
}
