namespace Didata_Assignment_Opdracht
{
    public class Program
    {
        /// <summary>
        /// The root function where the application starts.
        /// </summary>
        /// <param name="args">The supplied arguments to the application 
        /// via the console.</param>
        public static void Main(string[] args)
        {
            // Create an instance of the program to execute.
            Didata_Assignment_Opdracht program = new(args);
            try
            {
                // Execute the program.
                program.Execute();
            }
            catch (Exception e)
            {
                /// Handle a registered <see cref="Exception"/>.
                Console.WriteLine(e.Message);
                return;
            }

            if (Settings.isDebug)
            {
                /// Show data in the console, when set to debug in the 
                /// <see cref="Settings"/> class.
                program.ViewOrdersInConsole();
                Console.ReadLine();
            }
        }
    }
}
