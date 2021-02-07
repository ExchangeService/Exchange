using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Shared.Common
{
    internal sealed class FileReader : IFileReader
    {
        public string GetRelativeServerPath(string relativePath)
        {
            var location = Assembly.GetEntryAssembly()?.Location;
            if (location is null)
            {
                throw new InvalidOperationException("Could not get relative path");
            }

            return Path.Combine(location, relativePath);
        }

        public async Task<string> ReadFileAsync(string path)
        {
            await using var sourceStream = File.Open(path, FileMode.Open);
            var result = new byte[sourceStream.Length];
            await sourceStream.ReadAsync(result.AsMemory(0, (int)sourceStream.Length));

            return Encoding.ASCII.GetString(result);
        }
    }
}