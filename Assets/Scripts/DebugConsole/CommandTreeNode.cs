using System.Collections.Generic;
using System.Linq;

namespace Scripts.DebugConsole
{
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