using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using Scripts.Manager;
using Scripts.DebugConsole;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

namespace Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        public const string NAME = "@UI";
        

        private static UIManager instance;
        public static UIManager Instance
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

                    instance = root.AddComponent<UIManager>();
                    instance.init();
                }

                return instance;
            }
        }


        public void init()
        {
            DontDestroyOnLoad(gameObject);
        }


        public IEnumerator WaitForSelectButton(GameObject button)
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(button);
        }

        public void SelectButton(GameObject button) => EventSystem.current.SetSelectedGameObject(button);
        //StartCoroutine(WaitForSelectButton(button));

        public void SelectNavigate(InputActionReference navigation) =>
            ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move =
            InputActionReference.Create(navigation);

        public GameObject GetSelectedButton()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                return EventSystem.current.currentSelectedGameObject;
            return null;
        }

        public string GetSelectedButtonName()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                return EventSystem.current.currentSelectedGameObject.name;
            return null;
        }

        public string GetSelectedButtonDescription()
        {
            GameObject button = GetSelectedButton();
            if (button == null) return null;

            if (UIDB.buttonDescription.ContainsKey(button.name))
                return UIDB.buttonDescription[button.name];
            return null;
        }

        private string currentItemName;
        public string GetSelectedItemDescription()
        {
            GameObject button = GetSelectedButton();
            if (button == null) return null;

            string itemName;
            itemName = button.GetComponentInChildren<TextMeshProUGUI>().text;

            if (UIDB.allItemList.ContainsKey(itemName))
            {
                currentItemName = itemName;
                return UIDB.allItemList[itemName].description;
            }
            if (currentItemName == null) return null;
            return UIDB.allItemList[currentItemName].description;
        }

        public void ClearChildren(GameObject parent)
        {
            foreach(RectTransform child in parent.transform)
                Destroy(child.gameObject);
        }

        

        public int GetTabIndex(InputValue value, int now, int max)
        {
            int axis = (int)value.Get<float>();
            if (axis == 0) return -1;
            if (now + axis < 1 || now + axis > max) return -1;

            return now + axis;
        }

    }
}
