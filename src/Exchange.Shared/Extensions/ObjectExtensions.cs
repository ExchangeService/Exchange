using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Exchange.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetFilteredQueryParameters<TS, TAt>(
            this TS obj,
            bool encode = true,
            List<string>? selectedProperties = null,
            Func<TAt, string>? attributeValueProvider = null)
            where TAt : Attribute
        {
            var properties =
                from p in obj?.GetType()
                    .GetProperties()
                    .Where(e => selectedProperties is null || selectedProperties.Contains(e.Name))
                where p.GetValue(obj, null) != null
                select GetNameOrAttribute(p, attributeValueProvider)
                       + "="
                       + GetValue(obj, p, encode);

            return string.Join("&", properties.ToArray());
        }

        public static string GetQueryParameters<TS, TAt>(
            this TS obj,
            bool encode = true,
            List<string>? selectedProperties = null,
            Func<TAt, string>? attributeValueProvider = null)
            where TAt : Attribute
        {
            var properties =
                from p in obj?.GetType()
                    .GetProperties()
                    .Where(e => selectedProperties is null || !selectedProperties.Contains(e.Name))
                where p.GetValue(obj, null) != null
                select GetNameOrAttribute(p, attributeValueProvider)
                       + "="
                       + GetValue(obj, p, encode);

            return string.Join("&", properties.ToArray());
        }

        public static string? GetValue<TS>(
            this TS obj,
            PropertyInfo p,
            bool encode) =>
            encode ? HttpUtility.UrlEncode(p.GetValue(obj, null)?.ToString()) : p.GetValue(obj, null)?.ToString();

        private static string GetNameOrAttribute<TAt>(MemberInfo p, Func<TAt, string>? attributeValueProvider = null)
            where TAt : Attribute
        {
            if (attributeValueProvider is null)
            {
                return p.Name.ToLower();
            }

            var attribute = p.GetCustomAttribute<TAt>();
            return attribute is null ? p.Name.ToLower() : attributeValueProvider(attribute);
        }
    }
}