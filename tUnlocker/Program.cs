// -----------------------------------------------------------------------
//    tUnlocker - by atom0s (c) 2013 [atom0s@live.com]
//
//    tUnlocker is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    tUnlocker is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with tUnlocker.  If not, see <http://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------

namespace tUnlocker
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Terraria;

    internal static class Program
    {
        /// <summary>
        /// The location of Terraria.exe
        /// </summary>
        internal static string TerrariaPath;

        /// <summary>
        /// Terraria instance loaded after patching.
        /// </summary>
        internal static Assembly TerrariaAssembly;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Attach assembly resolve event handler..
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // Create and run the main form..
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var form = new frmMain())
            {
                var ret = form.ShowDialog();
                if (ret == DialogResult.Cancel)
                    return;

                // Process the selected option..
                Program.LaunchTerraria();
            }
        }

        /// <summary>
        /// Launches Terraria from memory.
        /// </summary>
        private static void LaunchTerraria()
        {
            using (var game = new Main())
            {
                Directory.SetCurrentDirectory(Program.TerrariaPath);
                game.Content.RootDirectory = Path.Combine(Program.TerrariaPath, "Content");
                game.Run();
            }
        }

        /// <summary>
        /// Adjusts the resolve path to attempt to locate the loading assembly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Obtain the resolving assemblyy name..
            var dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            // Ignore resources requests..
            if (dllName.EndsWith(".resources"))
                return null;

            // Return Terraria instance..
            if (args.Name.Contains("Terraria"))
                return Program.TerrariaAssembly;

            // Determine if this name matches a resource name..
            var resName = Assembly.GetExecutingAssembly().GetManifestResourceNames().SingleOrDefault(s => s.Contains(dllName));
            if (!string.IsNullOrEmpty(resName))
            {
                var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
                if (res == null)
                    return null;

                var data = new byte[res.Length];
                res.Read(data, 0, (int)res.Length);
                return Assembly.Load(data);
            }

            return null;
        }

        /// <summary>
        /// Gets a value from the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strKeyName"></param>
        /// <param name="strValueName"></param>
        /// <returns></returns>
        public static T GetRegistryValue<T>(String strKeyName, String strValueName)
        {
            try
            {
                return (T)Registry.GetValue(strKeyName, strValueName, default(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
