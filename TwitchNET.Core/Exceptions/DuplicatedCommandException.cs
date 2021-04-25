using System;
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

        public DuplicatedCommandException()
        {
        }

        public DuplicatedCommandException(string message) : base(message)
        {
        }

        public DuplicatedCommandException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DuplicatedCommandException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}