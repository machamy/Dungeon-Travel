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

namespace Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        public const string NAME = "@UI";

        [HideInInspector] public State currentState;
        public enum State
        {
            SelectBehaviour,
            BuyWeapon,
            AskBuyItem,
            Talk,
            AskExit,
            SelectPlace
        }

        public InputAction mainNavigation, yNavigation;

        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                // 없을경우 생성
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
            InputActionClass input = new InputActionClass();
            mainNavigation = input.MainUI.MainNavigate;
            yNavigation = input.MainUI.YNavigate;
        }


        public void SetUI(State state, GameObject enableUI, GameObject disableUI,
            GameObject firstSelectButton, bool isYNavigaion = false)
        {
            currentState = state;
            enableUI?.SetActive(true);
            disableUI?.SetActive(false);
            SelectButton(firstSelectButton);
            SelectNavigate(isYNavigaion);
        }

        public void SelectButton(GameObject button) =>
            StartCoroutine(WaitForSelectButton(button));

        public IEnumerator WaitForSelectButton(GameObject button)
        {
            yield return new WaitForSeconds(0.001f);
            EventSystem.current.SetSelectedGameObject(button);
        }

        public void SelectNavigate(bool isYNavigation) =>
            ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move =
            InputActionReference.Create(isYNavigation ? yNavigation : mainNavigation);

        public string GetSelectedButtonName()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                return EventSystem.current.currentSelectedGameObject.name;
            return null;
        }

        public string GetSelectedButtonDescription()
        {
            string buttonName;
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                buttonName = EventSystem.current.currentSelectedGameObject.name;
                if (UIDB.buttonDescription.ContainsKey(buttonName))
                    return UIDB.buttonDescription[buttonName];
            }
            return null;
        }
    }

}
