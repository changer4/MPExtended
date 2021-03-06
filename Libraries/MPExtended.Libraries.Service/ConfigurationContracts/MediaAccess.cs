﻿#region Copyright (C) 2011-2012 MPExtended
// Copyright (C) 2011-2012 MPExtended Developers, http://mpextended.github.com/
// 
// MPExtended is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPExtended is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPExtended. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MPExtended.Libraries.Service.ConfigurationContracts
{
    public enum ConfigType { File, Folder, Text, Number, Boolean }

    public class PluginConfigItem
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ConfigType Type { get; set; }

        public PluginConfigItem() 
        { 
        }

        public PluginConfigItem(PluginConfigItem old)
        {
            this.Value = old.Value;
            this.Name = old.Name;
            this.DisplayName = old.DisplayName;
            this.Type = old.Type;
        }
    }

    public class DefaultPlugins
    {
        public string TVShow { get; set; }
        public string Movie { get; set; }
        public string Music { get; set; }
        public string Picture { get; set; }
        public string Filesystem { get; set; }
    }

    public class MediaAccess
    {
        public DefaultPlugins DefaultPlugins { get; set; }
        public Dictionary<string, List<PluginConfigItem>> PluginConfiguration { get; set; }
        public List<string> DisabledPlugins { get; set; }

        public MediaAccess(string path, string defaultPath)
        {
            try
            {
                XElement file = XElement.Load(path);

                DefaultPlugins = new DefaultPlugins()
                {
                    Filesystem = file.Element("defaultPlugins").Element("filesystem").Value,
                    Movie = file.Element("defaultPlugins").Element("movie").Value,
                    Music = file.Element("defaultPlugins").Element("music").Value,
                    Picture = file.Element("defaultPlugins").Element("picture").Value,
                    TVShow = file.Element("defaultPlugins").Element("tvshow").Value,
                };

                DisabledPlugins = file.Element("disabledPlugins").Elements("disabled").Select(x => x.Value).ToList();

                PluginConfiguration = new Dictionary<string, List<PluginConfigItem>>();

                foreach (XElement plugin in file.Element("pluginConfiguration").Elements("plugin"))
                {
                    PluginConfiguration[plugin.Attribute("name").Value] = new List<PluginConfigItem>();
                    foreach(var x in plugin.Elements())
                    {
                        ConfigType type = (ConfigType)Enum.Parse(typeof(ConfigType), x.Attribute("type").Value, true);
                        string value = type == ConfigType.File || type == ConfigType.Folder ? Configuration.PerformFolderSubstitution(x.Value) : x.Value;
                        PluginConfiguration[plugin.Attribute("name").Value].Add(new PluginConfigItem()
                        {
                            DisplayName = x.Attribute("displayname").Value,
                            Name = x.Name.LocalName,
                            Type = type,
                            Value = value
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to load MediaAccess configuration", ex);
            }
        }

        public bool Save()
        {
            try
            {
                XElement file = XElement.Load(Configuration.GetPath("MediaAccess.xml"));

                file.Element("defaultPlugins").Element("tvshow").Value = DefaultPlugins.TVShow;
                file.Element("defaultPlugins").Element("movie").Value = DefaultPlugins.Movie;
                file.Element("defaultPlugins").Element("music").Value = DefaultPlugins.Music;
                file.Element("defaultPlugins").Element("picture").Value = DefaultPlugins.Picture;
                file.Element("defaultPlugins").Element("filesystem").Value = DefaultPlugins.Filesystem;

                file.Element("disabledPlugins").Elements("disabled").Remove();
                foreach (string item in DisabledPlugins)
                {
                    file.Element("disabledPlugins").Add(new XElement("disabled", item));
                }

                foreach (var pluginElement in file.Element("pluginConfiguration").Elements("plugin"))
                {
                    List<PluginConfigItem> newConfig = PluginConfiguration[pluginElement.Attribute("name").Value];
                    foreach (var configItem in pluginElement.Elements())
                    {
                        configItem.Value = newConfig.First(x => x.Name == configItem.Name.LocalName).Value;
                    }
                }

                file.Save(Configuration.GetPath("MediaAccess.xml"));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to write MediaAccess.xml", ex);
                return false;
            }
        }
    }
}
