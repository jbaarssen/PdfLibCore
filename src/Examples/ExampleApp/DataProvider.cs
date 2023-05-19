using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExampleApp
{
    public static class DataProvider
    {
        // ReSharper disable PossibleNullReferenceException
        private static string Name => typeof(DataProvider).FullName![..typeof(DataProvider).FullName!.IndexOf("Provider", StringComparison.Ordinal)];

        // This file is only here to resolve the assembly from the integration test projects
        // you can find the references by running Shift + F12 on the class
        // Please do not delete this file as long as there are references to it

        public static IEnumerable<string> GetManifestResourceNames() =>
            Assembly.GetAssembly(typeof(DataProvider))?.GetManifestResourceNames().Select(n => n[(Name.Length + 1)..]);

        /// <summary>
        /// Retrieves one of the embedded test files which can be used for various purposes
        /// </summary>
        /// <param name="resourceName">The name of the file as located in the "DataProvider" folder</param>
        /// <returns>The file in the form of a byte array</returns>
        public static async Task<byte[]> GetEmbeddedResourceAsync(string resourceName) =>
            (await GetEmbeddedResourceAsStreamAsync(resourceName)).ToArray();

        /// <summary>
        /// Retrieves one of the embedded test files which can be used for various purposes
        /// </summary>
        /// <param name="resourceName">The name of the file as located in the "DataProvider" folder</param>
        /// <returns>The file in the form of a stream</returns>
        public static async Task<MemoryStream> GetEmbeddedResourceAsStreamAsync(string resourceName)
        {
            await using var resourceStream = Assembly.GetAssembly(typeof(DataProvider))?.GetManifestResourceStream($"{Name}.{resourceName}")
                ?? throw new ArgumentException($"{resourceName} not found", nameof(resourceName));
            var memStream = new MemoryStream();
            await resourceStream.CopyToAsync(memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }
}