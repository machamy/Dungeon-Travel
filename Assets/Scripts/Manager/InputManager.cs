using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void menu();
    public static event menu Menu;
    public delegate void cancel();
    public static event cancel Cancel;
    public delegate void navigate();
    public static event navigate Navigate;

    public void OnMenu() => Menu?.Invoke();
    public void OnCancel() => Cancel?.Invoke();

    GameObject prevButton = null;
    private void Update()
    {
        if (prevButton != UIManager.Instance.GetSelectedButton())
        {
            prevButton = UIManager.Instance.GetSelectedButton();
            Navigate?.Invoke();
        }
    }
}
