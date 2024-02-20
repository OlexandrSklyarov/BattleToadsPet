using BT.Runtime.Data.Persistent;

namespace BT.Runtime.Services.Player
{
    public interface IPlayerDataStorageService
    {
        T GetData<T>() where T : IData;
    }
}