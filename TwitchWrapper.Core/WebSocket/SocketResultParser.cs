using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchWrapper.Core.Commands;

namespace TwitchWrapper.Core
{
    internal static class SocketResultParser
    {
        internal static async Task<string> GetResultAsync(ArraySegment<byte> buffer)
        {
            await using (var memoryStream = new MemoryStream(buffer.Array!))
            {
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}