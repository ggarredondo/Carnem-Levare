using System.Collections.Generic;

public class DataSaver : ISave
{
    private readonly XmlSerialize serializer;
    public static OptionsSlot options;
    public static List<GameSlot> games;
    public static int actualGameSlot;

    public DataSaver(SaveConfiguration config)
    {
        serializer = new();

        options = (OptionsSlot) config.defaultOptions.Clone();
        games = new List<GameSlot>(config.numGameSlots);

        for(int i = 0; i < games.Count; i++)
        {
            games[i] = (GameSlot) config.defaultGame.Clone();
            games[i].name += i + 1;
        }
    }

    public void Load()
    {
        options = (OptionsSlot) serializer.Load(options);
    }

    public void Save()
    {
        serializer.Save(options);
    }
}
