using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Exchange.Shared.Common
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string path);

        string GetRelativeServerPath(string relativePath);
    }
}