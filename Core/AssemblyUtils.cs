﻿// <copyright file="AssemblyUtils.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace AlgernonCommons
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ColossalFramework.Plugins;

    /// <summary>
    /// Core assembly-relateed utilities.
    /// </summary>
    public static class AssemblyUtils
    {
        // Mod assembly path cache.
        private static string s_assemblyPath = null;

        /// <summary>
        /// Gets current mod assembly name.
        /// </summary>
        public static string Name => Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// Gets current mod assembly version.
        /// </summary>
        public static Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Gets the current mod assembly version as a string, leaving off any trailing zero versions for build and revision.
        /// </summary>
        public static string TrimmedCurrentVersion => TrimVersion(CurrentVersion);

        /// <summary>
        /// Gets the mod directory filepath of the currently executing mod assembly.
        /// </summary>
        public static string AssemblyPath
        {
            get
            {
                // Return cached path if it exists.
                if (s_assemblyPath != null)
                {
                    return s_assemblyPath;
                }

                // No path cached - get list of currently active plugins.
                Assembly thisAssembly = Assembly.GetExecutingAssembly();
                IEnumerable<PluginManager.PluginInfo> plugins = PluginManager.instance.GetPluginsInfo();

                // Iterate through list.
                foreach (PluginManager.PluginInfo plugin in plugins)
                {
                    try
                    {
                        // Iterate through each assembly in plugins
                        foreach (Assembly assembly in plugin.GetAssemblies())
                        {
                            if (assembly == thisAssembly)
                            {
                                // Found it! Cache result and return path.
                                s_assemblyPath = plugin.modPath;
                                return plugin.modPath;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Don't care; just don't let a single failure stop us iterating through the plugins.
                        Logging.LogException(e, "exception iterating through plugins");
                    }
                }

                // If we got here, then we didn't find the assembly.
                Logging.Error("assembly path not found");
                return null;
            }
        }

        /// <summary>
        /// Returns the provided version as a string, leaving off any trailing zeros versions for build and revision.
        /// </summary>
        /// <param name="version">Version to display.</param>
        /// <returns>Trimmed version text.</returns>
        public static string TrimVersion(Version version)
        {
            // Trim off trailing zeros.
            if (version.Revision > 0)
            {
                // If any revision other than zero, we return the full version.
                return version.ToString(4);
            }
            else if (version.Build > 0)
            {
                // Revision is zero; if build is nonzero, return major.minor.build.
                // 1.0.1.0 => 1.0.1
                return version.ToString(3);
            }
            else
            {
                // Revision and build are zero; return major.minor.
                // 1.0.0.0 => 1.0
                return version.ToString(2);
            }
        }
    }
}
