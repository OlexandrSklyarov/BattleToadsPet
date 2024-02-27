using System;
using System.Collections.Generic;
using System.Diagnostics;
using BT.Runtime.Data.Configs;
using BT.Runtime.Data.Persistent;
using BT.Runtime.Services.Player;
using BT.Runtime.UI.Scenes;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BT.Runtime.Services.Levels
{
    public class SceneLoadService : ISceneLoadService
    {
        private readonly LevelDataBase _levelDataBase;
        private readonly PlayerDataStorageService _dataStorageService;
        private readonly UIElementConfig _uiElementConfig;

        public SceneLoadService(LevelDataBase levelDataBase, UIElementConfig uiElementConfig, PlayerDataStorageService dataStorageService)
        {
            _levelDataBase = levelDataBase;
            _dataStorageService = dataStorageService;
            _uiElementConfig = uiElementConfig;
        }

        public async UniTask LoadGameAsync()
        {
            var index = _dataStorageService.GetData<LevelData>().NextLevelIndex;
            var level = _levelDataBase.Levels[index];

            var loadingScreen = GetLoadingScreen();

            loadingScreen.Show();

            await SceneManager.LoadSceneAsync(level.Scene, LoadSceneMode.Single)
                .ToUniTask
                (
                    Progress.CreateOnlyValueChanged(progress => loadingScreen.SetProgress(progress, level.Scene), 
                    EqualityComparer<float>.Default)
                );

            await SceneManager.LoadSceneAsync(level.SceneEnvironment, LoadSceneMode.Additive)
                .ToUniTask
                (
                    Progress.CreateOnlyValueChanged(progress => loadingScreen.SetProgress(progress, level.SceneEnvironment), 
                    EqualityComparer<float>.Default)
                );

            loadingScreen.SetProgress(1f, "Completed");   
            loadingScreen.Hide();                      
        }

        private LoadingScreen GetLoadingScreen()
        {            
            var screen = UnityEngine.Object.FindFirstObjectByType<LoadingScreen>();
            
            if (screen == null)
                screen = UnityEngine.Object.Instantiate(_uiElementConfig.LoadingScreenPrefab);

            UnityEngine.Object.DontDestroyOnLoad(screen);
            
            return screen;
        }
    }
}