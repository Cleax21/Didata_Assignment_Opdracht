using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public class Argument
    {
        private readonly string[] Args;
        private Dictionary<ArgumentTypes, string> List = new();
        public Argument(string[] args)
        {
            Args = args;

            HandleArguments();
            ValidateArguments();
        }
        public Dictionary<ArgumentTypes, string> GetArguments()
        {
            return List;
        }

        private Dictionary<ArgumentTypes, string> HandleArguments()
        {
            for (int i = 0; i < Args.Length; i++)
            {
                switch (Args[i])
                {
                    case "--help":
                        List.Add(ArgumentTypes.HELP, "--help");
                        return List;
                    case "--h":
                        List.Add(ArgumentTypes.HELP, "--h");
                        return List;
                    case "--?":
                        List.Add(ArgumentTypes.HELP, "--?");
                        return List;
                    case "-d":
                        if (i + 1 >= Args.Length)
                        {
                            List.Add(ArgumentTypes.INVALID, $"Argument -d on index '{i}' is missing a directory path");
                            return List;
                        }
                        i++;
                        List.Add(ArgumentTypes.D, Args[i]);
                        break;
                    case "-f":
                        if (i + 1 >= Args.Length)
                        {
                            List.Add(ArgumentTypes.INVALID, $"Argument -f on index '{i}' is missing a .json file");
                            return List;

                        }
                        i++;
                        List.Add(ArgumentTypes.F, Args[i]);
                        break;
                    default:
                        if (Args.Last().Equals(Args[i]))
                        {
                            if (Args[i].Contains('-'))
                            {
                                List.Add(ArgumentTypes.INVALID, $"Unknown argument '{Args[i]}'. Use --help for more information");
                                return List;
                            }
                            List.Add(ArgumentTypes.FILENAME, Args.Last());
                        }
                        else
                        {
                            List.Add(ArgumentTypes.INVALID, $"Unknown argument '{Args[i]}'. Use --help for more information");
                            return List;
                        }
                        break;
                }
            }

            return List;
        }

        private void ValidateArguments()
        {
            if (List.ContainsKey(ArgumentTypes.HELP))
            {
                Help();
                throw new Exception("");
            }
            if (List.ContainsKey(ArgumentTypes.INVALID))
            {
                throw new Exception(List.FirstOrDefault(i => i.Key == ArgumentTypes.INVALID).Value);
            }

            bool foundArgD = false;
            bool foundArgF = false;

            if (List.ContainsKey(ArgumentTypes.D))
            {
                foundArgD = true;
                if (List.Where(i => i.Key == ArgumentTypes.D).Count() > 1)
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_ONLY_ONCE);
                }
            }

            if (List.ContainsKey(ArgumentTypes.F))
            {
                foundArgF = true;
            }

            if (!List.ContainsKey(ArgumentTypes.FILENAME))
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.NO_FILENAME_GIVEN);
            }

            if (foundArgD && foundArgF)
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_AND_F_INVALID);
            }
            else if (!foundArgD && !foundArgF)
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_OR_F_MISSING);
            }

            if (foundArgD && !Directory.Exists(List.FirstOrDefault(t => t.Key == ArgumentTypes.D).Value))
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_INVALID);
            }

            foreach (var item in List.Where(i => i.Key == ArgumentTypes.F))
            {
                if (Path.GetExtension(item.Value) != ".json")
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.INVALID_F_EXTENSION);
                }
                else if (!File.Exists(item.Value))
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.FILE_F_DOES_NOT_EXIST);
                }
            }

            if (Path.GetExtension(List.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value) != ".csv")
            {
                Console.WriteLine("filename doesn't include the valid .csv extension. Adding it manually...");
                List[List.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Key] = Path.ChangeExtension(List.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value, ".csv");
            }

            Console.WriteLine($"filename: {List.FirstOrDefault(i => i.Key == ArgumentTypes.FILENAME).Value}");


            if (foundArgD)
            {
                Console.WriteLine($"-d: {List.FirstOrDefault(i => i.Key == ArgumentTypes.D).Value}");
            }

            if (foundArgF)
            {
                foreach (var item in List.Where(i => i.Key == ArgumentTypes.F))
                {
                    Console.WriteLine($"-f: {item.Value}");
                }
            }

            Console.WriteLine("-----");
        }

        private void Help()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Syntax: Program-name [OPTIONS] FILENAME");
            Console.WriteLine("");
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("-d   Name of an existing directory where the");
            Console.WriteLine("     program searches for .json files. Can only be");
            Console.WriteLine("     used once. Exclusive to -f.");
            Console.WriteLine("");
            Console.WriteLine("-f   Name of the input file. Can be used multiple");
            Console.WriteLine("     times. Exclusive to -d");
            Console.WriteLine("");
            Console.WriteLine("FILENAME");
            Console.WriteLine("     Name of the output file");
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
