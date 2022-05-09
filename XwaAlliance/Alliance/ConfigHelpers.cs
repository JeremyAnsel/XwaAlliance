using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alliance
{
    static class ConfigHelpers
    {
        private const string ModNameFileName = "modname.txt";

        private const string ConfigFileName = "Config.cfg";

        private const string LastPilotKey = "lastpilot";

        private static readonly Encoding _encoding = Encoding.GetEncoding("iso-8859-1");

        private static readonly Func<string, bool> _isLastPilotKey = t => t.Equals(LastPilotKey) || t.StartsWith(LastPilotKey + " ");

        private static string GetPilotDirectory()
        {
            if (!File.Exists(ModNameFileName))
            {
                return ".";
            }

            string[] lines = File.ReadAllLines(ModNameFileName, _encoding);

            if (lines.Length < 1)
            {
                return ".";
            }

            return Path.Combine("UserData", lines[0], "Pilot");
        }

        public static List<string> GetPilotList()
        {
            var pilots = new List<string>();
            string pilotDirectory = GetPilotDirectory();

            if (!Directory.Exists(pilotDirectory))
            {
                return pilots;
            }

            foreach (string pilotPath in Directory.EnumerateFiles(pilotDirectory, "*.plt"))
            {
                string pilot = Path.GetFileNameWithoutExtension(pilotPath);
                pilot = pilot.Substring(0, pilot.Length - 1);

                pilots.Add(pilot);
            }

            pilots = pilots
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t)
                .ToList();

            return pilots;
        }

        public static string GetLastPilot()
        {
            if (!File.Exists(ConfigFileName))
            {
                return string.Empty;
            }

            string[] lines = File.ReadAllLines(ConfigFileName, _encoding);
            string lastPilotLine = lines.FirstOrDefault(_isLastPilotKey);

            if (string.IsNullOrEmpty(lastPilotLine) || lastPilotLine.Equals(LastPilotKey))
            {
                return string.Empty;
            }

            return lastPilotLine.Substring(LastPilotKey.Length + 1);
        }

        public static void SetLastPilot(string pilot)
        {
            string pilotKeyValue = string.IsNullOrEmpty(pilot) ? LastPilotKey : (LastPilotKey + " " + pilot);

            if (File.Exists(ConfigFileName))
            {
                List<string> lines = File.ReadAllLines(ConfigFileName, _encoding).ToList();
                int lineIndex = lines.FindIndex(new Predicate<string>(_isLastPilotKey));

                if (lineIndex == -1)
                {
                    lines.Insert(0, pilotKeyValue);
                }
                else
                {
                    lines[lineIndex] = pilotKeyValue;
                }

                File.WriteAllLines(ConfigFileName, lines.ToArray(), _encoding);
            }
            else
            {
                string[] lines = new string[]
                {
                    pilotKeyValue
                };

                File.WriteAllLines(ConfigFileName, lines, _encoding);
            }
        }

        public static void DeletePilot(string pilot)
        {
            string pilotDirectory = GetPilotDirectory();

            File.Delete(Path.Combine(pilotDirectory, pilot + "0.plt"));
        }
    }
}
