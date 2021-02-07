using System;
using System.Net;

using Convey.WebApi.Exceptions;

using Exchange.Shared.Language;

namespace Exchange.Shared.Exceptions
{
    public abstract class DefaultInternalizationExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
        {
            exception.SetCulture();
            return exception switch
            {
                ExternalException ex => new ExceptionResponse(
                    new ExceptionDetails
                    {
                        Code = ex.Code,
                        Reason = ex.Message
                    },
                    HttpStatusCode.BadRequest),
                DomainException ex => new ExceptionResponse(
                    new ExceptionDetails
                    {
                        Code = ex.Code,
                        Reason = ex.TranslationParameters.Length > 0
                                     ? string.Format(
                                         this.GetTranslation(ex.TranslationKey),
                                         ex.TranslationParameters as object?[])
                                     : this.GetTranslation(ex.TranslationKey)
                    },
                    HttpStatusCode.BadRequest),
                AppException ex => new ExceptionResponse(
                    new ExceptionDetails
                    {
                        Code = ex.Code,
                        Reason = ex.TranslationParameters.Length > 0
                                     ? string.Format(
                                         this.GetTranslation(ex.TranslationKey),
                                         ex.TranslationParameters as object?[])
                                     : this.GetTranslation(ex.TranslationKey)
                    },
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(
                    new
                    {
                        code = "error",
                        reason = "There was an error."
                    },
                    HttpStatusCode.BadRequest)
            };
        }

        protected abstract string GetTranslation(string key);
    }
}