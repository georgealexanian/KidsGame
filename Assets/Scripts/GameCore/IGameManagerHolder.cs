using System.Collections;

namespace GameCore
{
    public interface IGameManagerHolder
    {
        bool TryGetManager<T>(out T manager, GetManagerFilter filter) where T : class, IGameManager;
        IEnumerable GetAllManagers();
    }
}