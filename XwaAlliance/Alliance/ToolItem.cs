using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alliance
{
    public sealed class ToolItem
    {
        public ToolItem(string menuPath, string name, string path, string pathArguments, bool closeWindow)
        {
            this.MenuPath = string.IsNullOrEmpty(menuPath) ? string.Empty : menuPath;
            this.Name = string.IsNullOrEmpty(name) ? string.Empty : name;
            this.Path = string.IsNullOrEmpty(path) ? string.Empty : path;
            this.PathArguments = string.IsNullOrEmpty(pathArguments) ? string.Empty : pathArguments;
            this.CloseWindow = closeWindow;
        }

        public string MenuPath { get; }

        public string Name { get; }

        public string Path { get; }

        public string PathArguments { get; }

        public bool CloseWindow { get; }

        public bool IsMenu
        {
            get
            {
                return string.IsNullOrEmpty(Path);
            }
        }

        public bool IsPathDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                {
                    return false;
                }

                return Path.Equals(".", StringComparison.OrdinalIgnoreCase) || Path[Path.Length - 1] == '\\';
            }
        }

        public bool PathExists
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                {
                    return true;
                }

                if (IsPathDirectory)
                {
                    return System.IO.Directory.Exists(Path);
                }

                return System.IO.File.Exists(Path);
            }
        }

        public bool IsInMenu(string menu)
        {
            return MenuPath.Equals(menu, StringComparison.OrdinalIgnoreCase);
        }
    }
}
