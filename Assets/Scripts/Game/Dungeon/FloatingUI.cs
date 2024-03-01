using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatingUI : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    private RectTransform _rectTransform;
    [SerializeField]
    private TextMeshProUGUI _tmp;

    public TextMeshProUGUI TMP
    {
        get
        {
            if(!_tmp)
                _tmp = GetComponentInChildren<TextMeshProUGUI>();
            return _tmp;
        }
    }

    public string Text
    {
        get => _tmp.text;
        set => _tmp.text = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectsOfType<Camera>().First(c => c.name.ToLower().StartsWith("main"));
        Debug.Log(cam);
        _tmp = GetComponentInChildren<TextMeshProUGUI>();
        // _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        cam= FindObjectsOfType<Camera>().First(c => c.name.ToLower().StartsWith("main"));
        // 첫 프레임 이동
        if(target)
            MovePos();
    }

    
    void LateUpdate()
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
