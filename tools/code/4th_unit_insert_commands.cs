using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;

namespace i_commands
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 2)
            {
                Console.WriteLine("4th_unit_insert_commands.exe trans_path command_list");
                return;
            }

            foreach (string file in Directory.GetFiles(args[0], "*_filtered*"))
            {
                if (!Directory.Exists(args[0] + "\\merged"))
                    Directory.CreateDirectory(args[0] + "\\merged");


                //Get the culture property of the thread.
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                //Create TextInfo object.
                TextInfo textInfo = cultureInfo.TextInfo;

                string[] lines = File.ReadAllLines(file, Encoding.GetEncoding("SJIS"));



                string[] trans_commands = File.ReadAllLines(args[1], Encoding.GetEncoding("SJIS"));

                Dictionary<string, string> command_lookup = new Dictionary<string, string>();
                foreach (string trans_command in trans_commands)
                {
                    if (trans_command.Length > 0)
                    {
                        if (!command_lookup.Keys.Contains(trans_command.Split('=')[0]))
                        {
                            command_lookup.Add(trans_command.Split('=')[0], trans_command.Split('=')[1].Replace("/", "!"));
                        }
                    }
                }


                string output_name = file.Substring(0, file.Length - 4).Replace("translation", "translation\\merged") + ".txt";

                StreamWriter sw = new StreamWriter (output_name, false, Encoding.GetEncoding("SJIS"));
                // Get commands

                //foreach (string line in lines)
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length > 0)
                    {
                        if (lines[i].Length > 0 && lines[i].Contains(";C\"") && lines[i][0] == ';')
                        {
                            sw.WriteLine(lines[i]);

                            string command_list = lines[i].Substring(3, lines[i].Length - 5);

                            string[] commands = command_list.Split('/');

                            sw.Write("C\"");

                            for (int j = 0; j < commands.Length; j++)
                            {
                                if (commands[j][0] == ',')
                                {
                                    sw.Write(",");
                                    commands[j] = commands[j].Remove(0, 1);
                                }

                                if (command_lookup.ContainsKey(commands[j]))
                                {
                                    sw.Write(textInfo.ToTitleCase(command_lookup[commands[j]]) + "/");
                                }
                                else
                                {
                                    sw.Write(commands[j] + "/");
                                }
                            }

                            sw.WriteLine("\"");

                            if (i + 1 < lines.Length && lines[i + 1].Length > 0)
                            {
                                i++;
                            }

                        }
                        else if (lines[i].Length > 0 && lines[i].Contains(";L\"") && lines[i][0] == ';')
                        {
                            sw.WriteLine(lines[i]);

                            string command_list = lines[i];
                            string[] split = command_list.Split(new string[] { "L\"" },

                            StringSplitOptions.RemoveEmptyEntries);

                            for (int j = 1; j < split.Length; j++)
                            {

                                string command = split[j].Substring(0, split[j].IndexOf("\""));

                                sw.Write("L");

                                if (command_lookup.ContainsKey(command))
                                {
                                    sw.Write("\"" + textInfo.ToTitleCase(command_lookup[command]) + "\"");
                                }
                                else
                                {
                                    sw.Write("\"" + command + "\"");
                                }

                                sw.Write(split[j].Remove(0, command.Length + 1));
                            }

                            sw.WriteLine();

                            if (i + 1 < lines.Length && lines[i + 1].Length > 0)
                            {
                                i++;
                            }

                        }
                        else
                        {
                            sw.WriteLine(lines[i]);
                        }
                    }

                    else
                    {
                        sw.WriteLine(lines[i]);
                    }
                }

                sw.Close();

            }
        }
    }
}
