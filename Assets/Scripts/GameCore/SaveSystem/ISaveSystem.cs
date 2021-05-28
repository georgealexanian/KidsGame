namespace GameCore.SaveSystem
{
    public interface ISaveSystem:IGameManager
    {
        void Save();
        T TryGetSave<T>(bool childs = false);
        void AddSaveable(ISaveable saveable);

        void RemoveSaveable(ISaveable saveable);

        void RemoveAllSaveables();
        void DeleteSaveAndQuit();
    }
}
