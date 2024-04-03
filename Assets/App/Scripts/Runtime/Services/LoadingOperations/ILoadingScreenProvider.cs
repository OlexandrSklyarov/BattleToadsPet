using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Runtime.Services.LoadingOperations
{
    public interface ILoadingScreenProvider
    {
        UniTask Load(Queue<ILoadingOperation> operations);
    }
}

