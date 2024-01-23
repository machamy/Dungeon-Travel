using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
        private TMP_InputField input;
        
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
            commandFieldPrefab = Resources.Load<GameObject>("Prefebs/ConsoleInput");
            currentCanvas = FindObjectOfType<Canvas>();
            DontDestroyOnLoad(gameObject);
            initCommands();
        }

        private void initCommands()
        {
            CreateCommand("say",new DebugCommand("log",Debug.Log,"뒤의 모든 내용을 그대로 로깅한다."));
            CreateCommand(null, new DebugCommand("commands",() => Debug.Log("All commands :\n"+tree.GetAllCommandName())));
            CreateCommand(null, new DebugCommand("help",
                delegate(string fullPath)
                {
                    string tmp;
                    DebugCommand command = tree.getCommand(fullPath, out tmp);
                    if(command == null)
                        Debug.Log($"No command {fullPath}");
                    else
                        Debug.Log($"{command.Name}({command.ParamNum})\n" +
                                  $"{command.descripton}");
                }));
        }
        
        

        /// <summary>
        /// 명령어을 만들어 등록한다.
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
        public void CreateCommand(string path, DebugCommand command)
        {
            tree.Add(path, command);
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
                input = commandField.GetComponent<TMP_InputField>();
                input.onValueChanged.AddListener(OnValueChange);
                input.onEndEdit.AddListener(OnEndEdit);
            }
            else
            {
                input = commandField.GetComponent<TMP_InputField>();
                commandField.SetActive(true);
            }
            input.Select();
        }

        public void Disable()
        {
            state = false;
            commandField.SetActive(false);
            input.text = "";
        }


        private void OnValueChange(string value)
        {
            
        }
        private void OnEndEdit(string value)
        {
            Execute(value);
            //Disable();
        }
    }
}