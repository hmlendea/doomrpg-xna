using System;
using System.IO;
using System.Reflection;

namespace DoomRPG.Settings
{
    /// <summary>
    /// Application paths.
    /// </summary>
    public static class ApplicationPaths
    {
        static string rootDirectory;
        static string localAppData;

        /// <summary>
        /// The application directory.
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                if (rootDirectory == null)
                {
                    rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }

                return rootDirectory;
            }
        }

        /// <summary>
        /// Gets the user data directory.
        /// </summary>
        /// <value>The user data directory.</value>
        public static string UserDataDirectory
        {
            get
            {
                if (localAppData == null)
                {
                    localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                }

                return Path.Combine(localAppData, "GielinorAdventures");
            }
        }
        
        /// <summary>
        /// The data directory.
        /// </summary>
        public static string DataDirectory => Path.Combine(ApplicationDirectory, "Data");

        /// <summary>
        /// The entities directory.
        /// </summary>
        public static string EntitiesDirectory => Path.Combine(DataDirectory, "Entities");
        
        /// <summary>
        /// Gets the options file.
        /// </summary>
        /// <value>The options file.</value>
        public static string SettingsFile => Path.Combine(UserDataDirectory, "Settings.xml");
    }
}
