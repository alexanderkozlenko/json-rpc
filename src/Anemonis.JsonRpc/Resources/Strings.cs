﻿// © Alexander Kozlenko. Licensed under the MIT License.

using System.Globalization;
using System.Resources;

namespace Anemonis.JsonRpc.Resources
{
    internal static class Strings
    {
        private static readonly ResourceManager s_resourceManager =
            new ResourceManager(typeof(Strings).Namespace + "." + nameof(Strings), typeof(Strings).Assembly);

        public static string GetString(string name)
        {
            return s_resourceManager.GetString(name, CultureInfo.CurrentCulture);
        }
    }
}
