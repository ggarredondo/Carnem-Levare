using System.Collections.Generic;

public class DataSaver : ISave
{
    private readonly XmlSerialize serializer;
    public static OptionsSlot options;
    public static List<GameSlot> games;
    public static int currentGameSlot;

    public DataSaver(in SaveOptions configOptions, in SaveGame configGame)
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
}
