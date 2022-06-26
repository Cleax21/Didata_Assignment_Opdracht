using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// A class that processed given argument within a console application.
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// A collection of given arguments.
        /// </summary>
        private readonly string[] Args;

        /// <summary>
        /// A list of arguments in a more detailed <see cref="Dictionary{ArgumentTypes, string}"/> class.
        /// </summary>
        private Dictionary<ArgumentTypes, List<string>> List = new();

        private string? Filename;

        /// <summary>
        /// The constructor of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="args"></param>
        public Argument(string[] args)
        {
            // set args to class property.
            Args = args;

            // Process and validate the given arguments.
            HandleArguments();
            ValidateArguments();
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{ArgumentTypes, string}"/> list of arguments.
        /// </summary>
        /// <returns><see cref="Dictionary{ArgumentTypes, string}"/></returns>
        public Dictionary<ArgumentTypes, List<string>> GetArguments()
        {
            return List;
        }

        public string GetFilename()
        {
            return Filename ?? throw new Exception("Filename not set");
        }

        /// <summary>
        /// Processes the arguments within the <see cref="Argument"/> class.
        /// </summary>
        private void HandleArguments()
        {
            List.Add(ArgumentTypes.D, new List<string>());
            List.Add(ArgumentTypes.F, new List<string>());
            List.Add(ArgumentTypes.HELP, new List<string>());
            List.Add(ArgumentTypes.INVALID, new List<string>());

            /// Loops through the <see cref="Args"/> list.
            for (int i = 0; i < Args.Length; i++)
            {
                switch (Args[i])
                {
                    // User requests help.
                    case "--help":
                        List[ArgumentTypes.HELP].Add("--help");
                        return;
                    // User requests help.
                    case "--h":
                        List[ArgumentTypes.HELP].Add("--h");
                        return;
                    // User requests help.
                    case "--?":
                        List[ArgumentTypes.HELP].Add("--?");
                        return;
                    // User requests a -d argument.
                    case "-d":
                        if (i + 1 >= Args.Length)
                        {
                            // -d argument is missing a following directory path.
                            List[ArgumentTypes.INVALID].Add($"Argument -d on index '{i}' is missing a directory path");
                            return;
                        }
                        i++;
                        List[ArgumentTypes.D].Add(Args[i]);
                        break;
                    case "-f":
                        if (i + 1 >= Args.Length)
                        {
                            // -f argument is missing a following .json file.
                            List[ArgumentTypes.INVALID].Add($"Argument -f on index '{i}' is missing a .json file");
                            return;

                        }
                        i++;
                        // Add -f argument to list.
                        List[ArgumentTypes.F].Add(Args[i]);



                        break;
                    default:
                        // Check if item is the last item. (Required as last argument must be a FILENAME)
                        if (Args.Last().Equals(Args[i]))
                        {
                            if (Args[i].Contains('-'))
                            {
                                // Filename contains a '-' which is assumed as a argument.
                                List[ArgumentTypes.INVALID].Add($"Unknown argument '{Args[i]}'. Use --help for more information");
                                return;
                            }
                            // Filename is safe to use.
                            Filename = Args.Last();
                        }
                        else
                        {
                            List[ArgumentTypes.INVALID].Add($"Unknown argument '{Args[i]}'. Use --help for more information");
                            return;
                        }
                        break;
                }
            }

            return;
        }

        /// <summary>
        /// Validate all arguments within the <see cref="Argument.List"/> on errors or invalidations.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ValidateArguments()
        {
            // Checks if a HELP argument is present.
            if (List[ArgumentTypes.HELP].Count > 0)
            {
                Help();
                throw new Exception("");
            }
            // Checks if an INVALID argument is present.
            if (List[ArgumentTypes.INVALID].Count > 0)
            {
                throw new Exception(List[ArgumentTypes.INVALID].First());
            }

            // Boolean to check on existence of available arguments.
            bool foundArgD = false;
            bool foundArgF = false;

            // Checks if a -d argument is present.
            if (List[ArgumentTypes.D].Count > 0)
            {
                foundArgD = true;
                // Checks if more than 1 -d argument is present.
                if (List.Where(i => i.Key == ArgumentTypes.D).Count() > 1)
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_ONLY_ONCE);
                }
            }

            // Checks if a -f argument is present.
            if (List[ArgumentTypes.F].Count > 0)
            {
                foundArgF = true;
            }

            // Checks if a FILENAME is present.
            if (Filename == null)
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.NO_FILENAME_GIVEN);
            }

            // Checks if both -d and -f argument is present. (not allowed) 
            if (foundArgD && foundArgF)
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_AND_F_INVALID);
            }
            // Checks if neither -d or -f argument is present. (minimal of 1 of 2 argument required)
            else if (!foundArgD && !foundArgF)
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_OR_F_MISSING);
            }

            // Checks if the -d argument contains a valid directory path.
            if (foundArgD && !Directory.Exists(List[ArgumentTypes.D][0]))
            {
                ErrorHandler.writeErrorToConsole(ErrorTypes.ARGUMENT_D_INVALID);
            }

            foreach(var item in List[ArgumentTypes.F])
            {
                // Checks if the -f argument is a.json file. (mandatory)
                if (Path.GetExtension(item) != ".json")
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.INVALID_F_EXTENSION);
                }
                // If that is true, check if the file actually exists.
                else if (!File.Exists(item))
                {
                    ErrorHandler.writeErrorToConsole(ErrorTypes.FILE_F_DOES_NOT_EXIST);
                }
            }

            // Checks if the given FILENAME contain a .csv extension (mandatory, but changes it to one automatically)
            if (Path.GetExtension(Filename) != ".csv")
            {
                Console.WriteLine("filename doesn't include the valid .csv extension. Adding it manually...");
                HandleCSVFile();  
            }
            
            // If the application is in debug mode. print the valid arguments and FILENAME in the console output.
            if(Settings.isDebug)
            {
                Console.WriteLine($"filename: {Filename}");

                if (foundArgD)
                {
                    Console.WriteLine($"-d: {List.FirstOrDefault(i => i.Key == ArgumentTypes.D).Value}");
                }
                else if (foundArgF)
                {
                    Console.WriteLine("F Arguments:");
                    Console.WriteLine("----------");
                    foreach (var item in List.Where(i => i.Key == ArgumentTypes.F))
                    {
                        Console.WriteLine($"    -f: {item.Value}");
                        Console.WriteLine("----------");
                    }
                }
            }
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

        private void HandleCSVFile()
        {
            if (Filename != null)
            {
                Filename = Path.ChangeExtension(Filename, ".csv");
            }
        }
    }
}
