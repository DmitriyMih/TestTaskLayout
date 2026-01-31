using System.Collections.Generic;
using Sirenix.OdinInspector;
using TestTaskLayout.Infrastructure.Scenes;
using TestTaskLayout.Presentation.SplashScreen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTaskLayout.Composition
{
    public sealed class SplashScreenLifetimeScope : LifetimeScope
    {
        private float delaySeconds = 5f;

        [SerializeField, ValueDropdown(nameof(GetSceneKeys))]
        private string nextSceneKey = "None";

        private IEnumerable<string> GetSceneKeys() => SceneCatalogProvider.GetEnabledKeysFallback("Menu");

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SplashScreenManager>(Lifetime.Scoped);

            builder.RegisterInstance(delaySeconds);
            builder.RegisterInstance(nextSceneKey);

            builder.RegisterEntryPoint<SplashScreenEntryPoint>(Lifetime.Scoped);
        }
    }
}