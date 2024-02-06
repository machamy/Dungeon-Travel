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

        [HideInInspector] public UIDB.State currentState;
        

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

        
        public void SetUI(UIDB.State state, GameObject enableUI, GameObject disableUI,
            GameObject firstSelectButton)
        {
            currentState = state;
            enableUI?.SetActive(true);
            disableUI?.SetActive(false);
            SelectButton(firstSelectButton);
        }

        /*private bool cooltime = true; 
        public IEnumerator WaitForCooltime()
        {
            yield return new WaitForSeconds(0.001f);
            cooltime = true;
        }*/

        public void SelectButton(GameObject button) =>
            EventSystem.current.SetSelectedGameObject(button);

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

        public void GetInventoryItem(GameObject prefab, GameObject parent)
        {
            ClearChildren(parent);
            int posN = 0; int length = UIDB.inventoryItemList.Count;
            foreach (string itemName in UIDB.inventoryItemList)
            {
                GameObject buttonPrefab = Instantiate(prefab);
                buttonPrefab.transform.SetParent(parent.transform);

                Button button = buttonPrefab.GetComponentInChildren<Button>();
                Navigation navigation = button.navigation;

                buttonPrefab.name = itemName;
                buttonPrefab.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

                if (posN == 0)
                {
                    SelectButton(buttonPrefab.transform.GetChild(1).gameObject);
                }
                if (posN == length - 1)
                {
                    navigation.mode = Navigation.Mode.Explicit;
                    navigation.selectOnUp = buttonPrefab.transform.parent.
                        GetChild(length - 2).GetComponentInChildren<Button>();
                    button.navigation = navigation;
                }
                posN++;
            }


        }

    }
}
