using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _4th_unit_dat_build
{
    class _4th_unit_dat_build
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("4th_unit_dat_build.exe trans_pat orig_path");
                return;
            }

            string trans_dir = args[0];
            string orig_dir = args[1];

            string[] trans_files = Directory.GetFiles(trans_dir, "*_filtered*");
            //string[] orig_files = Directory.GetFiles(@"F:\transprojects\x68000\4th_unit\script_txt", "*.dat.txt");



            foreach (string trans_file in trans_files)
            {
                FileInfo fi = new FileInfo(trans_file);

                string[] trans_lines = File.ReadAllLines(trans_file, Encoding.GetEncoding("sjis"));

                string[] orig_lines = File.ReadAllLines(orig_dir + "\\" + fi.Name.Replace("_filtered", ""), Encoding.GetEncoding("SJIS"));

                StreamWriter combined = new StreamWriter(trans_file.Replace("_filtered", "").Replace(".txt", "").Replace("translation", "script_build"));


                int trans_pos = 0;
                for (int i = 0; i < orig_lines.Length; i++)
                {
                    if (orig_lines[i].Length > 0)
                    {
                        bool found = false;
                        string orig = orig_lines[i];
                        for (int j = trans_pos; j + 1 < trans_lines.Length; j++)
                        {


                            if (orig.Replace("\t", "") == trans_lines[j])
                            {
                                bool non_comment = false;
                                while (j + 1 < trans_lines.Length)
                                {
                                    if (trans_lines[j + 1].Length > 0 && trans_lines[j + 1][0] != ';')
                                    {
                                        non_comment = true;
                                        break;
                                    }
                                    else if (trans_lines[j + 1].Length == 0)
                                    {
                                        non_comment = false;
                                        break;
                                    }
                                    j++;
                                }

                                if (non_comment)
                                {
                                    if (trans_lines[j + 1].Length > 0 && trans_lines[j + 1][0] != ';')
                                    {
                                        combined.Write(trans_lines[j + 1]);
                                    }
                                    else
                                    {
                                        combined.Write(orig.Replace("\t", "").Replace(";", ""));
                                    }

                                    found = true;
                                    trans_pos = j;
                                    break;
                                }
                            }
                        }

                        if (!found)
                        {
                            combined.Write(orig.Replace("\t", "").Replace(";", ""));
                        }
                    }
                }
                combined.Close();


            }
        }
    }
}
