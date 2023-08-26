using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace i_commands
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("shrink INPUT_FILE");
                return;
            }

            string[] lines = File.ReadAllLines(args[0], Encoding.GetEncoding("SJIS"));


            string[] trans_commands = File.ReadAllLines("C:/Users/douglas.steele/Downloads/4th_Unit_Commands.txt", Encoding.GetEncoding("SJIS"));

            Dictionary<string, string> command_lookup = new Dictionary<string, string>();
            foreach (string trans_command in trans_commands)
            {
                command_lookup.Add(trans_command.Split('=')[0], trans_command.Split('=')[1]);
            }


            string output_name = args[0].Substring(0, args[0].Length - 4) + ".new.txt";

            StreamWriter sw = new StreamWriter(output_name, false, Encoding.GetEncoding("SJIS"));
            // Get commands

            //foreach (string line in lines)
            for(int i = 0; i < lines.Length; i++)
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
                                sw.Write(command_lookup[commands[j]] + "/");
                            }
                            else
                            {
                                sw.Write(commands[j] + "/");
                            }
                        }

                        sw.WriteLine("\"" + "\n");

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
            }

            sw.Close();


        }
    }
}
