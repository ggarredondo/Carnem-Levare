using System.Collections.Generic;

public interface IHaveText
{
    public string Text { get; }

    public int FontSize { get; }

    public float TimeBetweenChars { get; }

    public List<string> SoundsName { get; }

    public int EffectDistance { get; }
}
