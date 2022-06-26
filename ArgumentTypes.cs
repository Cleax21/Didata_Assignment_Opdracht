namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// A list of conditional statements which is used in class <see cref="Argument"/>.
    /// </summary>
    public enum ArgumentTypes
    {
        /// <summary>Used for '-f' argument</summary>.
        F,
        /// <summary>Used for '--help', '--h', OR '--?' argument</summary>.
        HELP,
        /// <summary>Used for invalid argument</summary>.
        INVALID
    }
}
