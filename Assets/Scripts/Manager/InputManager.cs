using Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public delegate void menu();
    public static event menu Menu;
    public delegate void cancel();
    public static event cancel Cancel;
    public delegate void navigate();
    public static event navigate Navigate;
    public delegate void xMove(InputValue value);
    public static event xMove XMove;

    public void OnMenu() => Menu?.Invoke();
    public void OnCancel() => Cancel?.Invoke();
    public void OnXMove(InputValue value) => XMove?.Invoke(value);

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
