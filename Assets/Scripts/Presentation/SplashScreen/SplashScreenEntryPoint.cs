using System;
using System.Threading;
using VContainer.Unity;

namespace TestTaskLayout.Presentation.SplashScreen
{
    public sealed class SplashScreenEntryPoint : IStartable, IDisposable
    {
        private readonly SplashScreenManager _manager;
        private readonly float _delaySeconds;
        private readonly string _nextSceneKey;

        private CancellationTokenSource _cts;

        public SplashScreenEntryPoint(
            SplashScreenManager manager,
            float delaySeconds,
            string nextSceneKey)
        {
            _manager = manager;
            _delaySeconds = delaySeconds;
            _nextSceneKey = nextSceneKey;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _ = _manager.RunAsync(_delaySeconds, _nextSceneKey, _cts.Token);
        }

        public void Dispose()
        {
            if (_cts == null) return;
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}