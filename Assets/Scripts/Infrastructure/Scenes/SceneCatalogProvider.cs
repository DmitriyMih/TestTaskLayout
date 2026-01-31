using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestTaskLayout.Infrastructure.Scenes
{
    public static class SceneCatalogProvider
    {
        private const string ResourceName = "SceneCatalog";
        private static SceneCatalog _cached;

        public static SceneCatalog Get()
        {
            if (_cached != null)
                return _cached;

            _cached = Resources.Load<SceneCatalog>(ResourceName);
            return _cached;
        }

        public static IEnumerable<string> GetEnabledKeysFallback(string fallbackKey = "SplashScreen")
        {
            var catalog = Get();
            if (catalog == null)
                return new[] { fallbackKey };

            var keys = catalog.GetEnabledKeys().ToArray();
            return keys.Length > 0 ? keys : new[] { fallbackKey };
        }
    }
}