
[System.Serializable]
[System.Xml.Serialization.XmlInclude(typeof(OptionsSlot))]
[System.Xml.Serialization.XmlInclude(typeof(GameSlot))]
public abstract class SaveSlot : System.ICloneable
{
    public string name;

    public object Clone()
    {
        return MemberwiseClone();
    }
}
