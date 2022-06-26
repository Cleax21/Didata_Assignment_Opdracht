namespace Didata_Assignment_Opdracht
{
    public static class Settings
    {
        // Debug options //

        /// <summary>
        /// Property of the application to run in debug mode if set to true.
        /// </summary>
        public static readonly bool isDebug = false;

        /// <summary>
        /// Property of the application to run a demonstration if set to true.
        /// </summary>
        public static readonly bool isDemonstration = false;

        // General Settings //

        /// <summary>
        /// Default location where the output file is getting stored.
        /// </summary>
        public static readonly string dataLocation = "\\data";

        /// <summary>
        /// Default filename on what the name of the output file will be.
        /// <para />
        /// Only works when <see cref="isDemonstration"/> is set to true.
        /// </summary>
        public static readonly string defaultFilename = "output.csv";

        /// <summary>
        /// Default foldername on where the .JSON files are stored.
        /// <para />
        /// Only works when <see cref="isDemonstration"/> is set to true.
        /// </summary>
        public static readonly string defaultFoldername = "\\Orders";
    }
}
