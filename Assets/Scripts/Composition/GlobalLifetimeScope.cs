using Sirenix.OdinInspector;
using TestTaskLayout.Infrastructure;
using TestTaskLayout.Infrastructure.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TestTaskLayout.Composition
{
    public sealed class GlobalLifetimeScope : LifetimeScope
    {
        [Title("Scenes")]
        [InfoBox("SceneCatalog is loaded automatically from Resources/SceneCatalog.asset")]
        [SerializeField, ReadOnly] private string catalogResourceName = "SceneCatalog";
        [SerializeField, ReadOnly] SceneCatalog catalog;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            catalog = SceneCatalogProvider.Get();
            if (catalog != null)
                builder.RegisterInstance(catalog);

            builder.Register<SceneNavigator>(Lifetime.Singleton);
        }
    }
}