namespace ProjectEclipsion.Core.Save;

public interface ISaveRepository
{
    void Save(SaveData saveData);

    bool TryLoad(out SaveData? saveData);
}
