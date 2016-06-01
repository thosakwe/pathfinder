using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;

namespace PathFinder
{
    public class ProgramInfo
    {
        /// <summary>
        /// The program's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The program's absolute path.
        /// </summary>
        public string Path { get; private set; }

        private BitmapImage icon_;

        /// <summary>
        /// An icon representing the program.
        /// </summary>
        public BitmapImage Icon
        {
            get
            {
                return icon_;
            }
        }

        private ProgramInfo()
        {

        }

        public static ProgramInfo FromFile(FileInfo file)
        {
            ProgramInfo programInfo = new ProgramInfo
            {
                Name = System.IO.Path.GetFileName(file.FullName),
                Path = file.FullName
            };

            // TODO: Load icons

            return programInfo;
        }
    }
}
