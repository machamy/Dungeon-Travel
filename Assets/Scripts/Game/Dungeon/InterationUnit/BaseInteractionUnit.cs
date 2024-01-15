using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum InteractionType
{
    None = 0,
    Use = 1<<0,
    Attack = 1<<1,
    All = 1<<8 - 1
}

public class BaseInteractionUnit : MonoBehaviour
{
    public InteractionType type;
    public float hp;

    private bool isFocused = false;
    private Material outline;
    public bool IsFocused
    {
        get => isFocused;
        set
        {
            isFocused = value;
            OnFocusEvent(value);
        }
    }
    
    public virtual void Start()
    {
        Shader shader = Shader.Find("Draw/OutlineShader");
        outline = new Material(shader);
    }
    
    public virtual void Update()
    {
        
    }

    public void OnUseEvent(PlayerUnit unit){
        
    }

    public void OnAttackEvent(PlayerUnit unit, float damage)
    {
        
    }

    private Renderer renderer;
    private List<Material> materialList = new List<Material>();
    public void OnFocusEvent(bool val)
    {
        renderer = GetComponent<Renderer>();
        
        materialList.Clear();
        materialList.AddRange(this.renderer.sharedMaterials);
        
        if (val)
            materialList.Add(outline);
        else
            materialList.Remove(outline);

        renderer.materials = materialList.ToArray();
    }
    
}

