using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData activeSave = new();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Load();
    }

    public void Save()
    {
        string dataPath = Application.persistentDataPath;

        XmlSerializer serializer = new(typeof(SaveData));
        FileStream stream = new(dataPath + "/" + activeSave.saveName + ".save", FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Saved");
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;

        if (File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            XmlSerializer serializer = new(typeof(SaveData));
            FileStream stream = new(dataPath + "/" + activeSave.saveName + ".save", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            activeSave.Load();

            Debug.Log("Loaded");
        }
    }

    public void DeleteSaveData()
    {
        string dataPath = Application.persistentDataPath;

        if (File.Exists(dataPath + "/" + activeSave.saveName + ".save"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".save");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName;

    //Audio Settings
    public float globalVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public bool mute = false;

    //Visual Settings
    public bool fullscreen;
    public int vsync;
    public string resolution;
    public int quality;

    public void Load()
    {
        AudioSaver.LoadChanges();
        VisualSaver.LoadChanges();
    }
}

