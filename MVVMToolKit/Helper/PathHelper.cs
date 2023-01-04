using System.IO;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The path helper class
    /// </summary>
    public class PathHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathHelper"/> class
        /// </summary>
        static PathHelper()
        {
            if (Directory.Exists(GetLocalDirectory()) is false)
            {
                Directory.CreateDirectory(GetLocalDirectory());
            }
        }

        /// <summary>
        /// Gets the local directory using the specified file name
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The string</returns>
        public static string GetLocalDirectory(string fileName) =>
            $"{GetLocalDirectory()}\\{fileName}";
        /// <summary>
        /// Gets the local directory
        /// </summary>
        /// <returns>The string</returns>
        public static string GetLocalDirectory() => $"{AppDomain.CurrentDomain.BaseDirectory}\\Data";


        /// <summary>
        /// Gets the local directory using the specified sub directory
        /// </summary>
        /// <param name="subDirectory">The sub directory</param>
        /// <param name="fileName">The file name</param>
        /// <returns>The string</returns>
        public static string GetLocalDirectory(string subDirectory, string fileName) =>
            $"{AppDomain.CurrentDomain.BaseDirectory}\\{subDirectory}\\{fileName}";
    }
}
