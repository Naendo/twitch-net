﻿using System;
using System.Runtime.Serialization;

namespace TwitchNET.Core.Exceptions
{
    [Serializable]
    public class DuplicatedCommandException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        
        public DuplicatedCommandException(string message) : base(message)
        {
        }
    }
}