using System;
using System.IO;
using System.Reflection;

namespace ExampleApp
{
    public static class DataProvider
    {
        // ReSharper disable PossibleNullReferenceException
        private static string Name => typeof(DataProvider).FullName![..typeof(DataProvider).FullName!.IndexOf("Provider", StringComparison.Ordinal)];

        // This file is only here to resolve the assembly from the integration test projects
        // you can find the references by running Shift + F12 on the class
        // Please do not delete this file as long as there are references to it

        /// <summary>
        /// Retrieves one of the embedded test files which can be used for various purposes
        /// </summary>
        /// <param name="resourceName">The name of the file as located in the "DataProvider" folder</param>
        /// <returns>The file in the form of a byte array</returns>
        public static byte[] GetEmbeddedResource(string resourceName) =>
            GetEmbeddedResourceAsStream(resourceName).ToArray();

        /// <summary>
        /// Retrieves one of the embedded test files which can be used for various purposes
        /// </summary>
        /// <param name="resourceName">The name of the file as located in the "DataProvider" folder</param>
        /// <returns>The file in the form of a stream</returns>
        public static MemoryStream GetEmbeddedResourceAsStream(string resourceName)
        {
            using var resourceStream = Assembly.GetAssembly(typeof(DataProvider))?.GetManifestResourceStream($"{Name}.{resourceName}");
            var memStream = new MemoryStream();
            resourceStream?.CopyTo(memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }
}