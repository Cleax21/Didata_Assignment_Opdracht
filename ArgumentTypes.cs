using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Didata_Assignment_Opdracht
{
    /// <summary>
    /// A list of conditional statements which is used in class <see cref="Argument"/>.
    /// </summary>
    public enum ArgumentTypes
    {
        /// <summary>Used for '-d' arguments</summary>.
        D,
        /// <summary>Used for '-d' duplicate arguments</summary>.
        D_Duplicate,
        /// <summary>Used for '-f' argument</summary>.
        F,
        /// <summary>Used for '--help', '--h', OR '--?' argument</summary>.
        HELP,
        /// <summary>Used for invalid argument</summary>.
        INVALID
    }
}
