namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// Represents all the arguments in which this application can use to 
    /// function properly.
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// A collection of given arguments from the user.
        /// </summary>
        private readonly string[] Args;

        /// <summary>
        /// A list of arguments in a more detailed 
        /// <see cref="Dictionary{ArgumentTypes, string}"/> class.
        /// </summary>
        private readonly Dictionary<ArgumentTypes, List<string>> List = new();

        /// <summary>
        /// The folder where all .JSON files are collected from.
        /// <para/>
        /// Default name (when <see cref="Settings.isDemonstration"/> is true) is set to
        /// <see cref="Settings.defaultFoldername"/>.
        /// </summary>
        private string? Foldername;

        /// <summary>
        /// The filename where the data is getting stored into.
        /// <para/>
        /// Default name when <see cref="Settings.isDemonstration"/> is true. is set to
        /// <see cref="Settings.defaultFilename"/>
        /// </summary>
        private string? Filename;

        /// <summary>
        /// The constructor of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="args">Given arguments from the user.</param>
        public Argument(string[] args)
        {
            Args = args;
        }

        /// <summary>
        /// Processes and validates the given arguments and returns a 
        /// <see cref="Dictionary{ArgumentTypes, string}"/> list of valid arguments.
        /// </summary>
        /// <returns><see cref="Dictionary{ArgumentTypes, string}"/>.</returns>
        public Dictionary<ArgumentTypes, List<string>> GetArguments()
        {
            /// Convert the raw <see cref="Args"/> data into known arguments.
            HandleArguments();

            // If the application has a demonstration settings enabled,
            // replace filename and foldername with the default one.
            if (Settings.isDemonstration)
            {
                Filename = Settings.defaultFilename;
                Foldername = Directory.GetCurrentDirectory() + Settings.defaultFoldername;
            }

            // Check if all arguments are valid. Otherwise throw an exception.
            ValidateArguments();

            return List;
        }

        /// <summary>
        /// Returns the <see cref="Filename"/> to where the data is getting stored.
        /// </summary>
        /// <returns>The FILENAME.</returns>
        public string GetFilename()
        {
            return Filename ?? string.Empty;
        }

        /// <summary>
        /// Returns the <see cref="Foldername"/> to where the .JSON files are stored.
        /// </summary>
        /// <returns>The input path where the files are located.</returns>
        public string GetFoldername()
        {
            return Foldername ?? string.Empty;
        }

        /// <summary>
        /// Processes the arguments within the <see cref="Argument"/> class.
        /// </summary>
        private void HandleArguments()
        {
            // Add all valid argumentTypes to the list that can be applied multiple times.
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
                            List[ArgumentTypes.INVALID].Add($"Argument -d on index " +
                                $"'{i}' is missing a directory path.");
                            return;
                        }
                        if (Foldername != null)
                        {
                            // -d argument is used more than once.
                            List[ArgumentTypes.INVALID].Add($"More than one -d argument" +
                                $" is present on index '{i}'.");
                            break;

                        }
                        i++;
                        Foldername = Args[i];
                        break;
                    case "-f":
                        if (i + 1 >= Args.Length)
                        {
                            // -f argument is missing a following .JSON file.
                            List[ArgumentTypes.INVALID].Add($"Argument -f on index " +
                                $"'{i}' is missing a .JSON file");
                            return;
                        }
                        i++;
                        // Add -f argument to list.
                        List[ArgumentTypes.F].Add(Args[i]);
                        break;
                    default:
                        // Check if item is the last item. (Required as last
                        // argument must be a FILENAME)
                        if (Args.Last().Equals(Args[i]))
                        {
                            if (Args[i].Contains('-'))
                            {
                                // Filename contains a '-', which is used to look like a unknown argument.
                                List[ArgumentTypes.INVALID].Add($"Unknown argument " +
                                    $"'{Args[i]}'. Use --help for more information");
                                return;
                            }
                            // Filename is safe to use.
                            Filename = Args.Last();
                        }
                        else
                        {
                            // Item is not the last item and is a unknown argument.
                            List[ArgumentTypes.INVALID].Add($"Unknown argument " +
                                $"'{Args[i]}'. Use --help for more information");
                            return;
                        }
                        break;
                }
            }

            return;
        }

        /// <summary>
        /// Validate all arguments within the <see cref="Argument.List"/> on 
        /// errors or invalidations.
        /// </summary>
        private void ValidateArguments()
        {
            // Validate help and invalid argument found within the List property.
            ValidateHelpArguments();
            ValidateInvalidArguments();

            // Check if a filename is given.
            ValidateFilename();

            // Check on all other properties.
            ValidateValidArguments();

            // If the application is in debug mode. print the valid arguments
            // and FILENAME in the console output.
            if (Settings.isDebug)
            {
                Console.WriteLine($"filename: {Filename}");

                if (Foldername != null)
                {
                    Console.WriteLine($"-d: {Foldername}");
                }
                else if (ArgumentTypeExists(ArgumentTypes.F))
                {
                    Console.WriteLine("F Arguments:");
                    Console.WriteLine("----------");
                    foreach (string item in List[ArgumentTypes.F])
                    {
                        Console.WriteLine($"    -f: {item}");
                        Console.WriteLine("----------");
                    }
                }
            }

        }

        /// <summary>
        /// Checks if the user has requested a help command in the terminal.
        /// </summary>
        /// <exception cref="Exception">Returns a list op helpful commands 
        /// for this application.</exception>
        private void ValidateHelpArguments()
        {
            // Checks if a HELP argument is present.
            if (ArgumentTypeExists(ArgumentTypes.HELP))
            {
                Help();
                throw new Exception("");
            }
        }

        /// <summary>
        /// Checks if the <see cref="List"/> contains any invalid arguments, 
        /// found when collecting the argument inside the 
        /// <see cref="HandleArguments"/> function.
        /// <para/>
        /// This also includes the part where neither -d and -f arguments can be used 
        /// simultaneously.
        /// </summary>
        /// <exception cref="Exception">When invalid arguments are 
        /// being used by the user.</exception>
        private void ValidateInvalidArguments()
        {
            // Checks if an INVALID argument is present.
            if (ArgumentTypeExists(ArgumentTypes.INVALID))
            {
                throw new Exception(List[ArgumentTypes.INVALID].First());
            }

            // Checks if both -d and -f argument is present. (not allowed) 
            if (Foldername != null && List[ArgumentTypes.F].Count > 0)
            {
                throw new Exception("Argument -d and -f argument can only be used exclusively.");
            }

            // Checks if neither -d or -f argument is present. (minimal of 1 of 2
            // argument required)
            if (List[ArgumentTypes.F].Count == 0 && Foldername == null)
            {
                throw new Exception("Argument -f OR -d is required.");
            }
        }

        /// <summary>
        /// Checks if all known arguments are valid.
        /// </summary>
        private void ValidateValidArguments()
        {
            ValidateDArguments();
            ValidateFArguments();
        }

        /// <summary>
        /// Checks if the argument -d is valid.
        /// </summary>
        /// <exception cref="Exception">Returns an error when the directory 
        /// path is invalid.</exception>
        private void ValidateDArguments()
        {
            // Checks if the -d argument is present.
            if (Foldername != null)
            {
                List.Remove(ArgumentTypes.F);

                // Checks if the -d argument contains a valid directory path.
                if (!Directory.Exists(Foldername))
                {
                    throw new Exception("Argument -d is invalid. Cannot find the directory path.");
                }
            }
        }

        /// <summary>
        /// Checks if all -f argument(s) are valid.
        /// </summary>
        /// <exception cref="Exception">Errors within the -f arguments.</exception>
        private void ValidateFArguments()
        {
            if (ArgumentTypeExists(ArgumentTypes.F))
            {
                Foldername = null;

                foreach (string item in List[ArgumentTypes.F])
                {
                    // Checks if the -f argument is a .JSON file. (mandatory)
                    if (Path.GetExtension(item) != ".json")
                    {
                        throw new Exception($"Argument -f '{item}' at index " +
                            $"'{List[ArgumentTypes.F].IndexOf(item) * 2}' " +
                            $"is invalid. all -f arguments need an .JSON extension.");
                    }
                    // If that is true, check if the file actually exists.
                    else if (!File.Exists(item))
                    {
                        throw new Exception($"Argument -f '{item}' at index " +
                            $"'{List[ArgumentTypes.F].IndexOf(item) * 2}' " +
                            $"does not exist.");
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a FILENAME property exists at the end of the arguments.
        /// <para />
        /// If a filename is given, but with the wrong / missing extension, 
        /// it will be changed to a .CSV file.
        /// </summary>
        /// <exception cref="Exception">Exception thrown when FILENAME is missing.</exception>
        private void ValidateFilename()
        {
            // Checks if a FILENAME is present.
            if (Filename == null)
            {
                throw new Exception("FILENAME is missing.");
            }

            // Checks if the given FILENAME contain a .CSV extension.
            // (mandatory, but changes it to one automatically)
            if (Path.GetExtension(Filename) != ".csv")
            {
                Console.WriteLine("FILENAME doesn't include the valid " +
                    ".CSV extension. Adding it manually...");
                HandleCSVFile();
            }
        }

        /// <summary>
        /// Validate if a <see cref="ArgumentTypes"/> within the <see cref="List"/> property exists.
        /// </summary>
        /// <param name="type">The <see cref="ArgumentTypes"/></param> that has to be checked.
        /// <returns><see cref="true"/> if found, <see cref="false"/> if not found.</returns>
        private bool ArgumentTypeExists(ArgumentTypes type)
        {
            return (List.ContainsKey(type) && List[type].Count > 0);
        }

        /// <summary>
        /// Prints out a helpful overview of all known commands and arguments.
        /// </summary>
        private static void Help()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Syntax: Program-name [OPTIONS] FILENAME");
            Console.WriteLine("");
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("-d   Name of an existing directory where the");
            Console.WriteLine("     program searches for .JSON files. Can only be");
            Console.WriteLine("     used once. Exclusive to -f.");
            Console.WriteLine("");
            Console.WriteLine("-f   Name of the input file. Can be used multiple");
            Console.WriteLine("     times. Exclusive to -d.");
            Console.WriteLine("");
            Console.WriteLine("--help   Returns this overview of commands.");
            Console.WriteLine("");
            Console.WriteLine("FILENAME");
            Console.WriteLine("     Name of the output file.");
            Console.WriteLine("--------------------------------------------------");
        }

        /// <summary>
        /// Changes the <see cref="Filename"/> to an .CSV file.
        /// </summary>
        private void HandleCSVFile()
        {
            if (Filename != null)
            {
                Filename = Path.ChangeExtension(Filename, ".csv");
            }
        }
    }
}
