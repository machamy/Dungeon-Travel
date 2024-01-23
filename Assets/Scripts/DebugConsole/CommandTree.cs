using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.DebugConsole
{
    public class CommandTree
    {
        private CommandTreeNode headerNode = new CommandTreeNode(null,null,null);

        public void Add(string path, Command command)
        {
            CommandTreeNode parent;
            if (path == null)
                parent = headerNode;
            else
                parent = FindNode(path, true);
            CommandTreeNode commandNode = new CommandTreeNode(parent, command.Name,command);
            Debug.Log($"Add command {command.Name} ({command.ParamNum})");
            parent.child.Add(command.Name, commandNode);
        }

        /// <summary>
        /// 해당 경로의 노드를 찾아 반환한다
        /// 경로가 없을경우 마지막 노드를 반환한다.
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="makePath">경로가 없을경우 생성 여부</param>
        /// <returns>마지막 노드</returns>
        public CommandTreeNode FindNode(string path, bool makePath = false)
        {
            CommandTreeNode result;
            string[] pathArr = path.Split(" ");
            CommandTreeNode current = headerNode;
            for (int i = 0; i < pathArr.Length; i++)
            {
                // 이미 경로가 있는경우
                CommandTreeNode find;
                if (current.child.TryGetValue(pathArr[i],out find))
                {
                    current = find;
                }
                // 경로가 없는경우
                else if (makePath)
                {
                    CommandTreeNode midPath = new CommandTreeNode(current,pathArr[i],null);
                    current.child[pathArr[i]] = midPath;
                    current = midPath;
                }
                else
                {
                    break;
                }
            }
            return current;
        }

        

        public Command getCommand(string fullPath, out string remainPath)
        {
            string[] pathArr = fullPath.Split(" ");
            CommandTreeNode current = headerNode;
            int i;
            for (i = 0; i < pathArr.Length; i++)
            {
                // 다음 경로가 있을경우 들어간다
                CommandTreeNode find;
                if (current.child.TryGetValue(pathArr[i], out find))
                {
                    current = find;
                }
                // 경로가 없는경우 끝낸다
                else
                {
                    break;
                }
            }
            //남은 경로 저장.
            remainPath = string.Join(" ", pathArr.Skip(i));
            return current.command;
        }
        
        public string GetAllCommandName()
        {
            StringBuilder sb = new StringBuilder();
            Stack<Tuple<int, CommandTreeNode>> q = new Stack<Tuple<int, CommandTreeNode>>();
            
            foreach (var nxtNode in headerNode.getChilds())
            {
                q.Push(new Tuple<int, CommandTreeNode>(0,nxtNode));
            }
            
            while (q.Any())
            {
                var tuple = q.Pop();
                int lv = tuple.Item1;
                var node = tuple.Item2;
                for (int i = 0; i < lv; i++)
                    sb.Append("-");
                sb.Append(node.ToString());
                sb.Append("\n");
                foreach (var nxtNode in node.getChilds())
                {
                    q.Push(new Tuple<int, CommandTreeNode>(lv+1,nxtNode));
                }
            }
            return sb.ToString();
        }
    }

    public class CommandTreeNode
    {
        public CommandTreeNode parent;
        public Dictionary<string, CommandTreeNode> child = new Dictionary<string, CommandTreeNode>();
        public Command command;
        public string name;

        internal CommandTreeNode(CommandTreeNode parent, string name, Command command)
        {
            this.parent = parent;
            this.command = command;
            this.name = name;
        }

        public List<CommandTreeNode> getChilds()
        {
            return child.Values.ToList();
        }

        public override string ToString()
        {
            if (command == null)
                return name;
            return $"{command.Name} ({command.ParamNum})";
        }
    

        public void Invoke(params string[] para) => command.Invoke(para);
    }
}