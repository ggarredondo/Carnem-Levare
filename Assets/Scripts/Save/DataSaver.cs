using System.Collections.Generic;

public class DataSaver : ISave
{
    private readonly XmlSerialize serializer;
    public static OptionsSlot options;
    public static List<GameSlot> games;
    public static int currentGameSlot;

    public DataSaver(in SaveConfiguration config)
    {
        serializer = new();

        options = (OptionsSlot) config.defaultOptions.Clone();
        games = new List<GameSlot>(config.numGameSlots);

        for (int i = 0; i < config.numGameSlots; i++)
        {
            games.Add((GameSlot) config.defaultGame.Clone());
            games[i].name += i + 1;
        }
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
}
