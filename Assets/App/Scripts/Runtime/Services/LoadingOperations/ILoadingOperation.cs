using System;
using Cysharp.Threading.Tasks;

namespace Game.Runtime.Services.LoadingOperations
{
    public interface ILoadingOperation
    {
        UniTask Load(Action<float> onProgressCallback);
    }
}

