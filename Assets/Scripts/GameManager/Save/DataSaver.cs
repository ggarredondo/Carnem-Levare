using System.Collections.Generic;
using UnityEngine.Audio;

public class DataSaver : ISave
{
    private readonly XmlSerialize serializer;
    private IApplier optionsApplier;

    public static OptionsSlot options;
    public static List<GameSlot> games;
    public static int currentGameSlot;

    public DataSaver(in SaveOptions configOptions, in SaveGame configGame, in AudioMixer audioMixer)
    {
        serializer = new();

        if(configOptions != null)
            options = (OptionsSlot) configOptions.defaultOptions.Clone();

        if (configGame != null)
        {
            games = new List<GameSlot>(configGame.numGameSlots);
            for (int i = 0; i < configGame.numGameSlots; i++)
            {
                games.Add((GameSlot)configGame.defaultGame.Clone());
                games[i].name += i + 1;
            }
        }

        optionsApplier = new OptionsApplier(audioMixer);
    }

    public static GameSlot CurrentGameSlot { get => games[currentGameSlot]; }

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
}
