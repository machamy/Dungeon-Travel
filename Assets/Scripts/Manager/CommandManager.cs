﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace Scripts.DebugConsole
{
    /// <summary>
    /// 명령어매니저, 싱글턴, MonoBehaviour
    /// </summary>
    public class CommandManager : MonoBehaviour
    {
        public const string NAME = "@Debug";
        
        private CommandTree tree;
        private GameObject commandFieldPrefab;
        public GameObject commandField;

        private ScrollRect scrollrect;
        private TMP_InputField input;
        private TextMeshProUGUI output;
        private GameObject completeObj;
        private TextMeshProUGUI completeText;
        
        
        public bool state = false;
        public bool debugConsoleOpt = true;
        public Canvas currentCanvas;

        private static CommandManager instance;
        public static CommandManager Instance
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

                    instance = root.AddComponent<CommandManager>();
                    instance.init();
                }

                return instance;
            }
        }
        
        private void init()
        {
            tree = new CommandTree();
            commandFieldPrefab = Resources.Load<GameObject>("Prefebs/Console");
            currentCanvas = FindObjectOfType<Canvas>();
            DontDestroyOnLoad(gameObject);
            initCommands();
        }

        private void initCommands()
        {
            CreateCommand(null, new Command("say", (s) => Print(s),"뒤의 모든 내용을 그대로 출력한다"));
            CreateCommand("say",new Command("log",Debug.Log,"뒤의 모든 내용을 그대로 로깅한다."));
            CreateCommand(null, new Command("cmds",() => Print("All commands :\n"+tree.GetAllCommandName())));
            CreateCommand(null, new Command("help",
                delegate(string fullPath)
                {
                    string tmp;
                    Command command = tree.getCommand(fullPath, out tmp);
                    if(command == null)
                        Print($"No command {fullPath}");
                    else
                        Print($"{command.Name}({command.ParamNum})\n" +
                              $"{command.descripton}");
                }));
            CreateCommand("search",new Command("cmds",str => Print(string.Join("\n", GetCommandsStartWith(str))), "해당 문자로 시작하는 모든 명령어 출력"));
        }
        
        

        /// <summary>
        /// 명령어를 만들어 등록한다.
        /// </summary>
        /// <remarks>
        /// <code>
        /// CreateCommand("say","log", command) // say log
        /// CreateCommand(null, "commands", command) // commands
        /// </code>
        /// <paramref name="path"/>가 say 이고 <paramref name="command"/>의 name이 log 인경우 say log 를 칠 경우 실행된다.
        /// </remarks>
        /// <param name="path">명령어의 경로</param>
        /// <param name="command">명령어</param>
        public void CreateCommand(string path, Command command)
        {
            tree.Add(path, command);
            tree.dirty = true; // 아직 처리 안함.
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                Toggle();
            }
        }
        
        public bool Execute(string fullCommand)
        {
            string para;
            var command = tree.getCommand(fullCommand,out para);
            if (command == null)
                return false;
            command.Invoke(para.Split(" "));
            return true;
        }

        public void Toggle()
        {
            if (!state)
                Enable();
            else
                Disable();
        }

        public void Enable()
        {
            state = true;
            
            if(!commandField)
            {
                commandField = Instantiate(commandFieldPrefab, currentCanvas.gameObject.transform);
                UpdateInputOutput();
                input.onValueChanged.AddListener(OnValueChange);
                input.onSubmit.AddListener(OnSubmit);
                input.onSelect.AddListener(OnSelct);
                input.onDeselect.AddListener(OnDeselect);
            }
            else
            {
                UpdateInputOutput();
            }
            commandField.SetActive(true);
            input.Select();
            OnSelct("");
        }

        public void Disable()
        {
            GameManager.Instance.PlayerActionMap.Enable();
            state = false;
            commandField.SetActive(false);
            input.text = "";
        }

        private void UpdateInputOutput()
        {
            input = commandField.GetComponentInChildren<TMP_InputField>();
            scrollrect = commandField.transform.GetComponentInChildren<ScrollRect>();
            output = scrollrect.transform.GetChild(0).GetChild(0)
                .GetComponentInChildren<TextMeshProUGUI>();
            completeObj = commandField.transform.GetChild(2).gameObject;
            completeText = completeObj.transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnValueChange(string value)
        {
            if (value == "")
                return;
            UpdateACtext(value);
        }
        

        private List<string> commandLog = new List<string>();
        private void OnSubmit(string value)
        {
            if (value == "")
                return;
            Print($"<color=grey>{value}</color>");
            commandLog.Add(value);
            
            bool result = Execute(value);
            input.text = "";
            if(!result)Print("<color=red>Not Valid Command! </color>");
            input.Select();
            ClearACtext();
            //Disable();
        }

        private void OnSelct(string value)
        {
            GameManager.Instance.PlayerActionMap.Disable();
        }

        private void OnDeselect(string value)
        {
            GameManager.Instance.PlayerActionMap.Enable();
            new InputActionClass().Enable();
        }


        /// <summary>
        /// 콘솔 창에 그대로 출력한다. 인자가 여러개일 경우 개행된다.
        /// </summary>
        /// <param name="str"></param>
        public void Print(params string[] str)
        {
            output.text += "\n" + string.Join("\n",str);
            StartCoroutine(GameManager.DoNextFrame(() => scrollrect.verticalNormalizedPosition = 0));
        }

        private void UpdateACtext(string value)
        {
            var Commands = GetCommandsStartWith(value);
            if (Commands.Count == 0)
            {
                ClearACtext();
                return;
            }
            completeObj.SetActive(true);
            completeText.text = string.Join("\n", Commands);
        }

        private void ClearACtext()
        {
            completeObj.SetActive(false);
            completeText.text = "";
        }

        public List<string> GetCommandsStartWith(string start)
        {
            Debug.Log(string.Join("\n",tree.GetAllPath()));
            var query = from e in tree.GetAllPath()
                where e.StartsWith(start)
                orderby e.Count(c=> c == ' '), e
                select e;
            return query.ToList();
        }
    }
}