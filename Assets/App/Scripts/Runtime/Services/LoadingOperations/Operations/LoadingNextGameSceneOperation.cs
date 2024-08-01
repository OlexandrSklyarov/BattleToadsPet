using System;
using System.Collections.Generic;
using BT.Runtime.Data;
using BT.Runtime.Data.Configs;
using BT.Runtime.Data.Persistent;
using BT.Runtime.Services.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Game.Runtime.Services.LoadingOperations.Operations
{
    public sealed class LoadingNextGameSceneOperation : ILoadingOperation
    {
         private readonly LevelDataBase _levelDataBase;
        private readonly PlayerDataStorageService _dataStorageService;

        public LoadingNextGameSceneOperation(LevelDataBase levelDataBase, PlayerDataStorageService dataStorageService)
        {
            _levelDataBase = levelDataBase;
            _dataStorageService = dataStorageService;
        }

        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0f);

            await LoadNextGameSceneAsync(onProgressCallback);

            var sec = DOTween.Sequence();
            sec.Append(DOVirtual.Float(0f, 0.7f, 0.5f, (p) => onProgressCallback?.Invoke(p)));
            await sec.AsyncWaitForCompletion();

            onProgressCallback?.Invoke(1f);
        }

        private async UniTask LoadNextGameSceneAsync(Action<float> onProgressCallback)
        {
            var index = _dataStorageService.GetData<LevelData>().NextLevelIndex;
            var level = _levelDataBase.Levels[index];

            await SceneManager.LoadSceneAsync(GameConstants.Scene.MEDIATOR_LEVEL_BOOT, LoadSceneMode.Single)
                .ToUniTask
                (
                    Progress.CreateOnlyValueChanged(progress => onProgressCallback?.Invoke(progress), 
                    EqualityComparer<float>.Default)
                );

            await SceneManager.LoadSceneAsync(level.Scene, LoadSceneMode.Single)
                .ToUniTask
                (
                    Progress.CreateOnlyValueChanged(progress => onProgressCallback?.Invoke(progress), 
                    EqualityComparer<float>.Default)
                );

            await SceneManager.LoadSceneAsync(level.SceneEnvironment, LoadSceneMode.Additive)
                .ToUniTask
                (
                    Progress.CreateOnlyValueChanged(progress => onProgressCallback?.Invoke(progress), 
                    EqualityComparer<float>.Default)
                );
           
        }
    }
}


