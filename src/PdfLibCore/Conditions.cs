using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PdfLibCore
{
    public static class Conditions
    {
        private const int MaxShortStringLength = 255;

        /// <summary>
        /// Ensures that <paramref name="value" /> is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to check, must not be null.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is null.</exception>
        public static T NotNull<T>(Expression<Func<T>> value, string message = null) where T : class => 
            NotNull(value.Compile().Invoke(), GetMemberName(value), string.IsNullOrEmpty(message) ? $"{GetMemberName(value)} must not be null" : message);

        /// <summary>
        /// Ensures that <paramref name="value" /> is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to check, must not be null.</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be blank</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name" /> is blank.</exception>
        public static T NotNull<T>(T value, string name) where T : class => 
            NotNull(value, name, $"{name} must not be null");

        /// <summary>
        /// Ensures that <paramref name="value" /> is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to check, must not be null.</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be blank</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" /> is null, must not be blank</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name" /> is blank.</exception>
        public static T NotNull<T>(T value, string name, string message) where T : class
        {
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            return value ?? throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> is not blank.
        /// </summary>
        /// <param name="value">The value to check, must not be blank.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is blank, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> or <paramref name="message" /> are blank.</exception>
        public static string NotBlank(Expression<Func<string>> value, string message) => 
            NotBlank(value.Compile().Invoke(), GetMemberName(value), message);

        /// <summary>
        /// Ensures that <paramref name="value" /> is not blank.
        /// </summary>
        /// <param name="value">The value to check, must not be blank.</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be
        /// blank.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is blank, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" />, <paramref name="name" />, or
        /// <paramref name="message" /> are blank.</exception>
        public static string NotBlank(string value, string name, string message)
        {
            message = !string.IsNullOrWhiteSpace(message) ? message : throw new ArgumentException($"{nameof(message)} must not be blank", nameof(message));
            name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException($"{nameof(name)} must not be blank", nameof(name));
            return !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Ensures that <paramref name="collection" /> contains at least one item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection to check, must not be null or empty.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="collection" />
        /// is empty, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collection" /> is empty, or if
        /// <paramref name="collection" /> or <paramref name="collection" /> are blank.</exception>
        public static IEnumerable<T> Any<T>(Expression<Func<IEnumerable<T>>> collection, string message) => 
            Any(collection.Compile().Invoke(), GetMemberName(collection), message);

        /// <summary>
        /// Ensures that <paramref name="collection" /> contains at least one item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection to check, must not be null or empty.</param>
        /// <param name="name">The name of the parameter the collection is taken from, must not be
        /// blank.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="collection" />
        /// is empty, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collection" /> is empty, or if
        /// <paramref name="collection" /> or <paramref name="name" /> are blank.</exception>
        public static IEnumerable<T> Any<T>(IEnumerable<T> collection, string name, string message)
        {
            // ReSharper disable PossibleMultipleEnumeration
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return collection == null || !collection.Any() ? throw new ArgumentException(message, name) : collection;
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> is true.
        /// </summary>
        /// <param name="value">The value to check, must be true.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is false, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is false
        /// or <paramref name="message" /> are blank.</exception>
        public static bool True(Expression<Func<bool>> value, string message) => 
            True(value.Compile().Invoke(), GetMemberName(value), message);

        /// <summary>
        /// Ensures that <paramref name="value" /> is true.
        /// </summary>
        /// <param name="value">The value to check, must be true.</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be blank.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is false, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is false, or if <paramref name="name" />
        /// or <paramref name="message" /> are blank.</exception>
        public static bool True(bool value, string name, string message)
        {
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return value ? true : throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> is false.
        /// </summary>
        /// <param name="value">The value to check, must be false.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is true, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is true
        /// or <paramref name="message" /> are blank.</exception>
        public static bool False(Expression<Func<bool>> value, string message) => 
            False(value.Compile().Invoke(), GetMemberName(value), message);

        /// <summary>
        /// Ensures that <paramref name="value" /> is false.
        /// </summary>
        /// <param name="value">The value to check, must be false.</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be blank.</param>
        /// <param name="message">The message to provide to the exception if <paramref name="value" />
        /// is true, must not be blank.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is true, or if <paramref name="name" />
        /// or <paramref name="message" /> are blank.</exception>
        public static bool False(bool value, string name, string message)
        {
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return !value ? false : throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> is less or equal than 255 characters.
        /// </summary>
        /// <param name="value">The value to check</param>
        public static string ShortString(Expression<Func<string>> value) => 
            ShortString(value.Compile().Invoke(), GetMemberName(value));

        /// <summary>
        /// Ensures that <paramref name="value" /> is less or equal than 255 characters.
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="name">The name of the parameter the value is taken from, must not be
        /// blank.</param>
        /// <exception cref="ArgumentException">Argument '{name}' must be less than or equal to {MaxShortStringLength}</exception>
        public static string ShortString(string value, string name) => 
            NotNull(value, name).Length > MaxShortStringLength ? throw new ArgumentException($"Argument '{name}' must be less than or equal to {MaxShortStringLength} characters.") : value;

        /// <summary>
        /// Ensures that the type is the expected type
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="message">The message.</param>
        public static TExpected TypeMatches<TExpected>(Expression<Func<object>> value, string message) => 
            TypeMatches<TExpected>(value.Compile().Invoke(), GetMemberName(value), message);

        /// <summary>
        /// Ensures that the type is the expected type
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public static TExpected TypeMatches<TExpected>(object value, string name, string message) => 
            (TExpected)TypeMatches(typeof(TExpected), value, name, message);

        /// <summary>
        /// Ensures that the type is the expected type
        /// </summary>
        /// <param name="expectedType"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static object TypeMatches(Type expectedType, object value, string name, string message)
        {
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return NotNull(expectedType, nameof(expectedType), $"{nameof(expectedType)} must not be null ").IsInstanceOfType(value) ?
                value : throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> is not equal to <paramref name="compareTo" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <param name="message">The message.</param>
        public static T NotEqualTo<T>(Expression<Func<T>> value, T compareTo, string message = null) => 
            (T)NotEqualTo(value.Compile().Invoke(), compareTo, GetMemberName(value), string.IsNullOrEmpty(message) ? $"{GetMemberName(value)} must not be null" : message);

        /// <summary>
        /// Ensures that <paramref name="value" /> is not equal to <paramref name="compareTo" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public static object NotEqualTo(object value, object compareTo, string name, string message)
        {
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return value == compareTo || value.Equals(compareTo) ? throw new ArgumentException(message, name) : value;
        }

        /// <summary>
        /// Ensures that <paramref name="value" /> matches with the <paramref name="patterns" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="message">The message.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns></returns>
        public static string Matches(Expression<Func<string>> value, string message, params string[] patterns) => 
            Matches(value.Compile().Invoke(), GetMemberName(value), message, patterns);

        /// <summary>
        /// Ensures that <paramref name="value" /> matches with the <paramref name="patterns" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <param name="patterns">The patterns.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">'{value}' does not match with pattern(s).</exception>
        public static string Matches(string value, string name, string message, params string[] patterns)
        {
            NotBlank(message, nameof(message), $"{nameof(message)} must not be blank");
            NotBlank(name, nameof(name), $"{nameof(name)} must not be blank");
            return patterns.Any(pattern => Regex.IsMatch(value, pattern)) ? value : throw new ArgumentException(message, nameof(message));
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private static string GetMemberName<T>(this Expression<T> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression m: return m.Member.Name;
                case UnaryExpression u when u.Operand is MemberExpression m: return m.Member.Name;
                default: throw new NotImplementedException(expression.GetType().ToString());
            }
        }
    }
}