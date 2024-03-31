using Scripts.Manager;
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
        //DontDestroyOnLoad(gameObject);
    }



    public Stack<GameObject> menuStack = new Stack<GameObject>();
    public Stack<GameObject> buttonStack = new Stack<GameObject>();

    public void PushUI(GameObject menu)
    {
        if (instance.menuStack.Count > 0)
            instance.menuStack.Peek().SetActive(false);

        menu.SetActive(true);
        instance.menuStack.Push(menu);

        instance.buttonStack.Push(UIManager.Instance.GetSelectedButton());
        SetDefaultButton(menu);
    }

    public void PopUI()
    {
        if (instance.menuStack.Count > 1)
        {
            instance.menuStack.Pop().SetActive(false);
            instance.menuStack.Peek().SetActive(true);

            UIManager.Instance.SelectButton(instance.buttonStack.Pop());
        }
    }

    private void SetDefaultButton(GameObject menu)
    {
        GameObject defaultButton = menu.GetComponentInChildren<Button>()?.gameObject;

        if (defaultButton != null)
            UIManager.Instance.SelectButton(defaultButton);
    }
}
