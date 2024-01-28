using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using Scripts.Manager;
using Scripts.DebugConsole;


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
        }


        public void SetUI(State state, GameObject enableUI, GameObject disableUI, GameObject firstSelectButton, InputActionReference navigation)
        {
            currentState = state;
            enableUI?.SetActive(true);
            disableUI?.SetActive(false);
            SelectButton(firstSelectButton);
            SelectNavigate(navigation);
        }

        public void SelectButton(GameObject button) =>
            StartCoroutine(WaitForSelectButton(button));

        public IEnumerator WaitForSelectButton(GameObject button)
        {
            yield return new WaitForSeconds(0.001f);
            EventSystem.current.SetSelectedGameObject(button);
        }

        public void SelectNavigate(InputActionReference navigation) =>
            ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move = navigation;

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
