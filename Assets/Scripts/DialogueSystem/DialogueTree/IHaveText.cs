using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogueTreeUtilities
{
    public interface IHaveText
    {
        public string Text { get; }

        public int FontSize { get; }

        public FontStyles Style { get; }

        public Color Color { get; }

        public float CharacterSpacing { get; }

        public float WordSpacing { get; }

        public TMP_ColorGradient ColorGradient { get; }

        public float TimeBetweenChars { get; }

        public List<string> SoundsName { get; }

        public SoundType SoundGenerationType { get; }

        public int EffectDistance { get; }
    }
}
