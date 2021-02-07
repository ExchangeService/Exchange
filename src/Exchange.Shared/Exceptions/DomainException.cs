using System;
using System.Globalization;

namespace Exchange.Shared.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message)
            : base(message) =>
            this.Lang = CultureInfo.CurrentCulture.Name;

        public abstract string Code { get; }

        public string Lang { get; }

        public abstract string TranslationKey { get; }

        public virtual string[] TranslationParameters => Array.Empty<string>();
    }
}
