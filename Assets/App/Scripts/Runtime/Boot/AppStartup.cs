using System.Linq;
using BT.Runtime.Services.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace BT.Runtime.Boot
{
    public class AppStartup : MonoBehaviour
    {
        private ISceneLoadService _sceneLoadService;

        [Inject]
        private void Construct(ISceneLoadService sceneLoadService)
        {
            _sceneLoadService = sceneLoadService;
        }

        private async void Start()
        {
            await _sceneLoadService.LoadGameAsync();

            var startup = SceneManager.GetSceneByName("Game").GetRootGameObjects()
                .Select(x => x.GetComponent<EcsStartup>())
                .FirstOrDefault();

            startup.Init();
        }
    }
}
