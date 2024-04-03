using System.Collections.Generic;
using System.Linq;
using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay;
using BT.Runtime.Services.Player;
using Cysharp.Threading.Tasks;
using Game.Runtime.Services.LoadingOperations;
using Game.Runtime.Services.LoadingOperations.Operations;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace BT.Runtime.Boot
{
    public sealed class AppStartup : MonoBehaviour
    {
        private ILoadingScreenProvider _loadingScreenProvider;
        private LevelDataBase _levelDataBase;
        private PlayerDataStorageService _dataStorageService;

        [Inject]
        private void Construct(ILoadingScreenProvider loadingScreenProvider,
            PlayerDataStorageService dataStorageService,
            LevelDataBase levelDataBase)
        {
            _loadingScreenProvider = loadingScreenProvider;
            _levelDataBase = levelDataBase;
            _dataStorageService = dataStorageService;
        }

        private async void Start()
        {
            var operations = GetLoadingOperations();

            await _loadingScreenProvider.Load(operations);

            var startup = SceneManager.GetSceneByName("Game").GetRootGameObjects()
                .Select(x => x.GetComponent<EcsStartup>())
                .FirstOrDefault();

            startup.Init();
        }

        private Queue<ILoadingOperation> GetLoadingOperations()
        {
            var queue = new Queue<ILoadingOperation>();

            queue.Enqueue(new LoadingNextGameSceneOperation(_levelDataBase, _dataStorageService));

            return queue;
        }
    }
}
