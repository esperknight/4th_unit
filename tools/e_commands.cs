using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace e_ecommands
{
    class e_commands
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("shrink INPUT_FILE");
                return;
            }

            string[] lines = File.ReadAllLines(args[0], Encoding.GetEncoding("SJIS"));


            // Get commands

            List<string> commands = new List<string>();

            foreach (string line in lines)
            {
                if (line.Length > 0 && line.Contains(";C\""))
                {
                    string command_list = line.Substring(3, line.Length - 5).Replace(",", "");

                    foreach (string command in command_list.Split('/'))
                    {
                        if (!commands.Contains(command))
                        {
                            commands.Add(command);
                        }
                    }
                }
            }

            DirectoryInfo info = Directory.GetParent(args[0]);

            string output_name = info.FullName + "\\4th_Unit_Commands.txt";

            string[] existing_commands = new string[0];

            if (File.Exists(output_name))
            {
                existing_commands = File.ReadAllLines(output_name, Encoding.GetEncoding("SJIS"));
            }

            for (int i = 0; i < existing_commands.Length; i++)
            {
                if (existing_commands[i].Contains("="))
                {
                    existing_commands[i] = existing_commands[i].Split('=')[0];
                }
            }

            StreamWriter command_out = new StreamWriter(output_name, true, Encoding.GetEncoding("SJIS"));

            foreach (string command in commands)
            {
                if (!existing_commands.Contains(command))
                {
                    command_out.WriteLine(command + "=" + command);
                }
            }

            command_out.Close();

        }
    }
}
