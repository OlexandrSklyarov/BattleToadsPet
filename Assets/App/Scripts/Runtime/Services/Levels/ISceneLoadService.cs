using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BT.Runtime.Services.Levels
{
    public interface ISceneLoadService
    {
        UniTask LoadGameAsync();
    }
}