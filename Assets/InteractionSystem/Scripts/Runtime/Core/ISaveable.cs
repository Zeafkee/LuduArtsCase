namespace LuduInteraction.Runtime.Core
{
    public interface ISaveable
    {
        string SaveID { get; }
        void LoadState(bool state);
        bool SaveState();
    }
}
