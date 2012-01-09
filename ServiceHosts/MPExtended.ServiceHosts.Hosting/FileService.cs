#region Copyright (C) 2011-2012 MPExtended
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
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using MPExtended.Libraries.General;

namespace MPExtended.ServiceHosts.Hosting
{
    class FileService : IFileService
    {
        public Stream GetSilverlightPolicy()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            return ReadFile("clientaccesspolicy.xml");
        }

        protected Stream ReadFile(string file)
        {
            string path = "";
            if (Installation.GetFileLayoutType() == FileLayoutType.Source)
            {
                path = Path.Combine(Installation.GetSourceRootDirectory(), "ServiceHosts", "MPExtended.ServiceHosts.Hosting", file);
            }
            else
            {
                path = Path.Combine(Installation.GetInstallDirectory(MPExtendedProduct.Service), file);
            }

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }
    }
}
