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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using MPExtended.Libraries.Service;
using MPExtended.Libraries.Service.Util;
using MPExtended.Applications.ServiceConfigurator.Code;

namespace MPExtended.Applications.ServiceConfigurator.Pages
{
    /// <summary>
    /// Interaction logic for TabProject.xaml
    /// </summary>
    public partial class TabProject : Page
    {
        private BackgroundWorker versionChecker;

        public TabProject()
        {
            InitializeComponent();
            lblVersion.Text = VersionUtil.GetVersionName();
        }

        private void hbUpdates_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            tbVersion.Text = String.Format("{0} ({1})", VersionUtil.GetVersionName(), Strings.UI.CheckingForUpdates);
            e.Handled = true;

            versionChecker = new BackgroundWorker();
            versionChecker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                string text;
                if (!UpdateChecker.IsWorking())
                {
                    text = Strings.UI.FailedToRetrieveUpdateInformation;
                }
                else if (UpdateChecker.IsUpdateAvailable())
                {
                    text = String.Format(Strings.UI.UpdateAvailable, UpdateChecker.GetLastReleasedVersion().Version, UpdateChecker.GetLastReleasedVersion().ReleaseDate);
                }
                else
                {
                    text = Strings.UI.NoUpdateAvailable;
                }

                args.Result = String.Format("{0} ({1})", VersionUtil.GetVersionName(), text);
            };
            versionChecker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                tbVersion.Text = args.Result as string;
            };
            versionChecker.RunWorkerAsync();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            CommonEventHandlers.NavigateHyperlink(sender, e);
        }
    }
}

