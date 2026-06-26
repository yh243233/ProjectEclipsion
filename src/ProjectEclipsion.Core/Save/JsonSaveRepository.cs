using System;
using System.IO;
using System.Text.Json;

namespace ProjectEclipsion.Core.Save;

public sealed class JsonSaveRepository : ISaveRepository
{
    private readonly string saveFilePath;

    public JsonSaveRepository(string saveFilePath)
    {
        if (string.IsNullOrWhiteSpace(saveFilePath))
        {
            throw new ArgumentException("Save file path is required.", nameof(saveFilePath));
        }

        this.saveFilePath = saveFilePath;
    }

    public void Save(SaveData saveData)
    {
        ArgumentNullException.ThrowIfNull(saveData);

        var directory = Path.GetDirectoryName(saveFilePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        File.WriteAllText(saveFilePath, json);
    }

    public bool TryLoad(out SaveData? saveData)
    {
        saveData = null;
        if (!File.Exists(saveFilePath))
        {
            return false;
        }

        try
        {
            var json = File.ReadAllText(saveFilePath);
            saveData = JsonSerializer.Deserialize<SaveData>(json);
            return saveData is not null;
        }
        catch (IOException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
