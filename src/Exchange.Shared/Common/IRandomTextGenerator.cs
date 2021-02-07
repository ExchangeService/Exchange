using JetBrains.Annotations;

namespace Exchange.Shared.Common
{
    public interface IRandomTextGenerator
    {
        [NotNull]
        string Generate(int length, [NotNull] string availableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
    }
}