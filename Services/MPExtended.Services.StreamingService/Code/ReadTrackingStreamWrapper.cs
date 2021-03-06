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
using System.IO;
using System.Diagnostics;

namespace MPExtended.Services.StreamingService.Code
{
    // This might be the worst code I've ever written
    internal class ReadTrackingStreamWrapper : Stream
    {
        private Stream wrappedStream;
        private Stopwatch readTimer;

        public ReadTrackingStreamWrapper(Stream toWrap)
        {
            wrappedStream = toWrap;
            readTimer = new Stopwatch();
            readTimer.Start();
        }

        public long TimeSinceLastRead
        {
            get { return readTimer.ElapsedMilliseconds; }
        }

        public override bool CanRead
        {
            get { return wrappedStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return wrappedStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return wrappedStream.CanWrite; }
        }

        public override long Length
        {
            get { return wrappedStream.Length; }
        }

        public override long Position
        {
            get
            {
                return wrappedStream.Position;
            }
            set
            {
                wrappedStream.Position = value;
            }
        }

        public override void Flush()
        {
            wrappedStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            readTimer.Restart();
            return wrappedStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return wrappedStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            wrappedStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            wrappedStream.Write(buffer, offset, count);
        }
    }
}
