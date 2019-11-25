﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Libawai.Infrastructure.Services;

namespace Libawai.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            IPropertyMapping propertyMapping)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertyMapping == null)
            {
                throw new ArgumentNullException(nameof(propertyMapping));
            }

            var mappingDictionary = propertyMapping.MappingDictionary;
            if (mappingDictionary ==  null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);
                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                if (!mappingDictionary.TryGetValue(propertyName, 
                    out var mappedProperties))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                if (mappedProperties == null)
                {
                    throw new ArgumentNullException(propertyName);
                }

                mappedProperties.Reverse();

                foreach (var destinationProperty in mappedProperties)
                {
                    if (destinationProperty.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.OrderBy(destinationProperty.Name +
                                            (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }

        public static IQueryable<object> ToDynamicQueryable<TSource>
            (this IQueryable<TSource> source, string fields, Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(fields))
            {
                return (IQueryable<object>) source;
            }

            fields = fields.ToLower();
            var fieldsAfterSpit = fields.Split(',').ToList();
            if (!fieldsAfterSpit.Contains("id", StringComparer.InvariantCultureIgnoreCase))
            {
                fieldsAfterSpit.Add("id");
            }

            var selectClause = "new (";

            foreach (var field in fieldsAfterSpit)
            {
                var propertyName = field.Trim();
                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                var key = mappingDictionary.Keys.SingleOrDefault(k =>
                    string.CompareOrdinal(k.ToLower(), propertyName.ToLower()) == 0);
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var mappedProperties = mappingDictionary[key];
                if (mappedProperties == null)
                {
                    throw new ArgumentNullException(key);
                }

                foreach (var destinationProperty in mappedProperties)
                {
                    selectClause += $" {destinationProperty.Name},";
                }
            }

            selectClause = selectClause.Substring(0, selectClause.Length - 1) + ")";
            return (IQueryable<object>)source.Select(selectClause);
        }
    }
}
