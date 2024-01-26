using System;
using UnityEngine;

namespace Scripts.DebugConsole
{
    public class Command
    {
        public string Name { get; private set; }
        public int ParamNum { get; private set; }

        public string descripton;
        /// <summary>
        /// 자동완성에 사용됨
        /// </summary>
        public string[][] arguments = new string[5][];
        
        /// <summary>
        /// 인자가 없는 명령어
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public Command(string name, Action action, string descripton = "설명이 없습니다.")
        {
            Name = name;
            ParamNum = 0;
            _action0 = action;
            this.descripton = descripton;
        }

        /// <summary>
        /// 인자가 1개인 명령어
        /// 띄어쓰기가 있을경우 그대로 전부 받는다.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public Command(string name, Action<string> action, string descripton = "설명이 없습니다.")
        {
            Name = name;
            ParamNum = 1;
            _action1 = action;
            this.descripton = descripton;
        }
        
        /// <summary>
        /// 인자가 2개인 명령어
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public Command(string name, Action<string, string> action, string descripton = "설명이 없습니다.")
        {
            Name = name;
            ParamNum = 2;
            _action2 = action;
            this.descripton = descripton;
        }
        /// <summary>
        /// 인자가 3개인 명령어
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public Command(string name, Action<string, string, string> action, string descripton = "설명이 없습니다.")
        {
            Name = name;
            ParamNum = 3;
            _action3 = action;
            this.descripton = descripton;
        }

        private Action _action0;
        private Action<string> _action1;
        private Action<string,string> _action2;
        private Action<string,string,string> _action3;

        public void Invoke(params string[] para)
        {
            if (para.Length < ParamNum)
            {
                errormsg.Invoke();
                return;
            }
            switch (ParamNum)
            {
                case 0:
                    _action0.Invoke();
                    break;
                case 1 :
                    _action1.Invoke(string.Join(" ",para));
                    break;
                case 2:
                    _action2.Invoke(para[0],para[1]);
                    break;
                case 3:
                    _action3.Invoke(para[0],para[1],para[2]);
                    break;
            }
        }

        public Command SetDescription(params string[] description)
        {
            this.descripton = string.Join("\n",descripton);
            return this;
        }

        /// <summary>
        /// 자동완성에 사용될 인자들을 지정한다.
        /// </summary>
        /// <param name="idx">인자의 순서</param>
        /// <param name="arguments">인자의 이름</param>
        /// <returns></returns>
        public Command SetAugments(int idx = 0, params string[] arguments)
        {
            this.arguments[idx] = arguments;
            return this;
        }

        public static Command errormsg = new Command("ERROR", ()=> Debug.Log("Not Valid Command"));
    }
}