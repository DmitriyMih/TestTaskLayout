using System.Threading;
using System.Threading.Tasks;
using TestTaskLayout.Infrastructure;

namespace TestTaskLayout.Presentation.SplashScreen
{
    public sealed class SplashScreenManager
    {
        private readonly SceneNavigator _sceneNavigator;

        public SplashScreenManager(SceneNavigator sceneNavigator)
        {
            _sceneNavigator = sceneNavigator;
        }

        public async Task RunAsync(float delaySeconds, string nextSceneKey, CancellationToken ct)
        {
            if (delaySeconds > 0f)
                await Task.Delay((int)(delaySeconds * 1000f), ct);

            ct.ThrowIfCancellationRequested();
            await _sceneNavigator.LoadByKeyAsync(nextSceneKey);
        }
    }
}