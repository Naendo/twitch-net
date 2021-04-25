using System;
using System.Runtime.Serialization;

namespace TwitchWrapper.Core.Exceptions.TypeReaderException
{
    [Serializable]
    public class TypeReaderException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        internal TypeReaderException(Type typeReader, string input) : base(
            $"TypeReader of type {typeReader.FullName} can not parse input: [{input}].")
        {
        }


        protected TypeReaderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}