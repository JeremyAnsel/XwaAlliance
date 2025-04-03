using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alliance
{
    public sealed class ToolItemList
    {
        public List<ToolItem> Items { get; } = new List<ToolItem>();

        public static ToolItemList FromLines(string[] lines)
        {
            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            var list = new ToolItemList();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("#") || line.StartsWith(";") || line.StartsWith("//"))
                {
                    continue;
                }

                string[] parts = line.Split('|');

                string menuPath;
                string name;
                string path;
                string pathArguments;
                bool closeWindow;

                if (parts.Length == 3)
                {
                    menuPath = string.Empty;
                    name = parts[0].Trim();

                    int nameIndex = name.IndexOf('\\');
                    if (nameIndex != -1)
                    {
                        menuPath = name.Substring(0, nameIndex);
                        name = name.Substring(nameIndex + 1);
                    }

                    path = parts[1].Trim();
                    pathArguments = string.Empty;

                    if (bool.TryParse(parts[2].Trim(), out bool result))
                    {
                        closeWindow = result;
                    }
                    else
                    {
                        closeWindow = false;
                    }
                }
                else if (parts.Length == 4)
                {
                    menuPath = string.Empty;
                    name = parts[0].Trim();

                    int nameIndex = name.IndexOf('\\');
                    if (nameIndex != -1)
                    {
                        menuPath = name.Substring(0, nameIndex);
                        name = name.Substring(nameIndex + 1);
                    }

                    path = parts[1].Trim();
                    pathArguments = parts[2].Trim();

                    if (bool.TryParse(parts[3].Trim(), out bool result))
                    {
                        closeWindow = result;
                    }
                    else
                    {
                        closeWindow = false;
                    }
                }
                else
                {
                    continue;
                }

                list.Items.Add(new ToolItem(menuPath, name, path, pathArguments, closeWindow));
            }

            return list;
        }

        public List<ToolItem> GetMenuItems(string menuPath)
        {
            var items = new List<ToolItem>();

            foreach (ToolItem item in Items)
            {
                if (item.IsInMenu(menuPath))
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
