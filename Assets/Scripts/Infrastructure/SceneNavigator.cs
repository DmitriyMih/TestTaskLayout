using System.Threading.Tasks;
using TestTaskLayout.Infrastructure.Scenes;
using UnityEngine.SceneManagement;

namespace TestTaskLayout.Infrastructure
{
    public sealed class SceneNavigator
    {
        private readonly SceneCatalog _catalog;

        public SceneNavigator(SceneCatalog catalog)
        {
            _catalog = catalog;
        }

        public async Task LoadByKeyAsync(string sceneKey)
        {
            if (_catalog == null)
                return;

            if (!_catalog.TryGetSceneName(sceneKey, out var sceneName))
                return;

            var op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone)
                await Task.Yield();
        }
    }
}