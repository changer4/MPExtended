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
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using MPExtended.Libraries.General;

namespace MPExtended.ServiceHosts.Hosting
{
    internal class BaseAddresses
    {
        public static Uri[] GetForService(MPExtendedService service)
        {
            return GetForService(service.ToString());
        }

        public static Uri[] GetForService(string serviceName)
        {
            return GetForPostfix(String.Format("MPExtended/{0}/", serviceName));
        }

        public static Uri[] GetForPostfix(string postfix)
        {
            List<Uri> ret = new List<Uri>() { };

            // HTTP binding: pick the first non-local address to make sure there is also an valid IP in the SOAP messages.
            // We're restricted to just one IP by the WCF hosting, can't do anything about that. Shipping IIS is a bit too much. 
            var addresses = NetworkInformation.GetIPAddressList();
            var nonLocalAddresses = NetworkInformation.GetIPAddressList().Where(x => !NetworkInformation.IsLocalAddress(x));
            IPAddress addr;
            if (addresses.Any(x => !NetworkInformation.IsLocalAddress(x)))
            {
                addr = addresses.First(x => !NetworkInformation.IsLocalAddress(x));
            }
            else
            {
                addr = addresses.First();
            }
            ret.Add(new Uri(String.Format("http://{0}:{1}/{2}", addr, Configuration.Services.Port, postfix)));

            // local net.pipe binding
            ret.Add(new Uri(String.Format("net.pipe://127.0.0.1/{0}", postfix)));

            return ret.ToArray();
        }
    }
}
