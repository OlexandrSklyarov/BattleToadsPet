using System.Collections.Generic;
using BT.Runtime.Data.Configs;
using Cysharp.Threading.Tasks;
using Game.Runtime.Services.LoadingOperations.View;

namespace Game.Runtime.Services.LoadingOperations
{
    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private readonly UIElementConfig _config;
        private LoadingScreenView _loadingScreen;

        public LoadingScreenProvider(UIElementConfig config)
        {
            _config = config;

        }

        public async UniTask Load(Queue<ILoadingOperation> operations)
        {
            var loadingScreen = GetLoadingScreen();
            await loadingScreen.LoadAsync(operations);
        }

        private LoadingScreenView GetLoadingScreen()
        {
            return _loadingScreen ??= UnityEngine.Object.Instantiate(_config.LoadingScreenPrefab);
        }        
    }
}
