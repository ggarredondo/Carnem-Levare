using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DataSaver : ISave
{
    private readonly XmlSerialize serializer;
    private IApplier optionsApplier;
    private readonly int maxGameSlot;

    private static OptionsSlot options;
    private static List<GameSlot> games;
    private static int currentGameSlot;

    private SaveGame defaultGameData;
    private SaveOptions defaultOptionsData;

    public static GameSlot Game { get => games[currentGameSlot]; }
    public static OptionsSlot Options { get => options; }

    public DataSaver(in SaveOptions configOptions, in SaveGame configGame, in AudioMixer audioMixer)
    {
        serializer = new();

        defaultOptionsData = configOptions;
        defaultGameData = configGame;

        if (configOptions != null)
        {
            configOptions.Configure();
            options = (OptionsSlot)configOptions.defaultOptions.Clone();
        }

        if (configGame != null)
        {
            maxGameSlot = configGame.numGameSlots - 1;
            games = new List<GameSlot>(configGame.numGameSlots);
            for (int i = 0; i < configGame.numGameSlots; i++)
            {
                games.Add((GameSlot)configGame.defaultGame.Clone());
                games[i].name += i + 1;
            }
        }

        optionsApplier = new OptionsApplier(audioMixer);
    }

    public void Load()
    {
        options = (OptionsSlot) serializer.Load(options);
        //games[currentGameSlot] = (GameSlot) serializer.Load(games[currentGameSlot]);
    }

    public void Save()
    {
        serializer.Save(options);
        //serializer.Save(games[currentGameSlot]);
    }

    public void ApplyChanges()
    {
        optionsApplier.ApplyChanges(options);
    }

    public void ChangeGameSlot(int slot)
    {
        if (slot >= 0 && slot <= maxGameSlot) currentGameSlot = slot;
        else Debug.LogWarning("The Game Slot is not valid");
    }

    public bool IsLoaded()
    {
        return games != null;
    }

    public void SetDefault()
    {
        games[currentGameSlot] = (GameSlot)defaultGameData.defaultGame.Clone();
    }
}
