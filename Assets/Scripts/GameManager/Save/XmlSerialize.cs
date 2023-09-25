using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class XmlSerialize
{
    public void Save(in SaveSlot save)
    {
        string dataPath = Application.persistentDataPath;

        XmlSerializer serializer = new(typeof(SaveSlot));
        FileStream stream = new(dataPath + "/" + save.name + ".save", FileMode.Create);
        serializer.Serialize(stream, save);
        stream.Close();
    }

    public SaveSlot Load(in SaveSlot save)
    {
        string dataPath = Application.persistentDataPath;
        SaveSlot newSave = save;

        if (File.Exists(dataPath + "/" + save.name + ".save"))
        {
            XmlSerializer serializer = new(typeof(SaveSlot));
            FileStream stream = new(dataPath + "/" + save.name + ".save", FileMode.Open);
            newSave = serializer.Deserialize(stream) as SaveSlot;
            stream.Close();
        }
        else Debug.LogWarning(save.name + " don't exist, no files to load");

        return newSave;
    }

    public void DeleteSaveSlot(in SaveSlot save)
    {
        string dataPath = Application.persistentDataPath;

        if (File.Exists(dataPath + "/" + save.name + ".save"))
        {
            File.Delete(dataPath + "/" + save.name + ".save");
        }
    }
}
