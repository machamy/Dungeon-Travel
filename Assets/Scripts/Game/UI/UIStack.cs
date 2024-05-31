using Scripts.Manager;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStack : MonoBehaviour
{
    public const string NAME = "@UI";

    private static UIStack instance;
    public static UIStack Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject root = GameObject.Find(NAME);
                if (root == null)
                {
                    root = new GameObject { name = NAME };
                }

                instance = root.AddComponent<UIStack>();
                instance.init();
            }

            return instance;
        }
    }

    public void init()
    {
        DontDestroyOnLoad(gameObject);
    }



    public Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject currentUI
    {
        get => instance.menuStack.Peek();
        set
        {
            instance.menuStack.Push(value);
            value.SetActive(true);
        }
    }

    public void PushUI(GameObject menu)
    {
        if (instance.menuStack.Count >= 1)
            currentUI.SetActive(false);

        currentUI = menu;

        //instance.buttonStack.Push(UIManager.Instance.GetSelectedButton());
        //SetDefaultButton(currentUI);
    }

    public void PopUI()
    {
        if (instance.menuStack.Count == 1)
        {
            return;
        }

        instance.menuStack.Pop().SetActive(false);
        currentUI.SetActive(true);

        SetDefaultButton(currentUI);
    }

    private void SetDefaultButton(GameObject menu)
    {
        GameObject defaultButton = menu.GetComponentInChildren<Button>()?.gameObject;

        if (defaultButton != null)
            UIManager.Instance.SelectButton(defaultButton);
    }
}
