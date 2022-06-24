using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    public static class ErrorHandler
    {
        public static void writeErrorToConsole(ErrorTypes type)
        {
            switch (type)
            {
                case ErrorTypes.NO_ARGUMENT:
                    Console.WriteLine("No arguments has been supplied. Use --help for further information.");
                    break;
                case ErrorTypes.ARGUMENT_D_ONLY_ONCE:
                    Console.WriteLine("Argument -d can only be used once.");
                    break;
                case ErrorTypes.ARGUMENT_D_AND_F_INVALID:
                    Console.WriteLine("Both -d and -f argument can only be used exclusively.");
                    break;
                case ErrorTypes.INVALID_ARGUMENTS:
                    Console.WriteLine("The given arguments are invalid. type --help for available arguments");
                    break;
                case ErrorTypes.ARGUMENT_D_OR_F_MISSING:
                    Console.WriteLine("Argument -f OR -d is missing, which is mandatory.");
                    break;
                case ErrorTypes.ARGUMENT_D_INVALID:
                    Console.WriteLine("Argument -d is invalid. Cannot find the directory path.");
                    break;
                case ErrorTypes.INVALID_F_EXTENSION:
                    Console.WriteLine("A -f argument is invalid. all -f argument need an .json extension");
                    break;
                case ErrorTypes.FILE_F_DOES_NOT_EXIST:
                    Console.WriteLine($"A -f file does not exist.");
                    break;
                case ErrorTypes.NO_FILENAME_GIVEN:
                    Console.WriteLine($"FILENAME is missing.");
                    break;
                default:
                    Console.WriteLine("Something went wrong. Not sure what");
                    break;
            }
            System.Environment.Exit(0);
        }
    }
}
