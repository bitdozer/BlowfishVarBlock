using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlowfishVarBlock
{
    /****************************************************************************
     |
     | Copyright (c) 2007 Novell, Inc.
     | All Rights Reserved.
     |
     | This program is free software; you can redistribute it and/or
     | modify it under the terms of version 2 of the GNU General Public License as
     | published by the Free Software Foundation.
     |
     | This program is distributed in the hope that it will be useful,
     | but WITHOUT ANY WARRANTY; without even the implied warranty of
     | MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     | GNU General Public License for more details.
     |
     | You should have received a copy of the GNU General Public License
     | along with this program; if not, contact Novell, Inc.
     |
     | To contact Novell about this file by physical or electronic mail,
     | you may find current contact information at www.novell.com 
     |
     |  Author: Russ Young
     |	Thanks to: Bruce Schneier / Counterpane Labs 
     |	for the Blowfish encryption algorithm and
     |	reference implementation. http://www.schneier.com/blowfish.html
     |
     |  Adapted from Ishan Jain's implementation on github:
     |  https://github.com/b1thunt3r/blowfish-csharp
     |
     |  Variable block size code added by Richard Diamond (@bitdozer)
     |
     |***************************************************************************/

    /// <summary>
    /// Class that provides blowfish encryption.
    /// </summary>
    /// <see cref="https://github.com/b1thunt3r/blowfish-csharp/blob/master/Simias.Encryption/BlowFish.cs"/>
    public class Blowfish : BlowfishVarBlock.PCL.Blowfish
    {
        /// <summary>
        /// Constructs and initializes a blowfish instance with the supplied key.
        /// </summary>
        /// <param name="key">The key to cipher with.</param>
        public Blowfish(byte[] key) : base(key) { }
    }

}
