using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alliance
{
    public sealed class ToolItem
    {
        public ToolItem(string name, string path, bool closeWindow)
        {
            this.Name = name;
            this.Path = path;
            this.CloseWindow = closeWindow;
        }

        public string Name { get; }

        public string Path { get; }

        public bool CloseWindow { get; }
    }
}
