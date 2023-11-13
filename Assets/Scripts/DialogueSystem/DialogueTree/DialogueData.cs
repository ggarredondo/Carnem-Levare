using TMPro;
using UnityEngine;

namespace DialogueTreeUtilities { 
    [System.Serializable]
    public class DialogueData
    {
        [Header("Text Settings")]
        public int fontSize;
        public Color color;
        public TMP_ColorGradient colorGradient;
        public FontStyles style;
        public float characterSpacing;
        public float wordSpacing;

        [Header("Speed")]
        [Range(0f, 0.4f)] public float timeBetweenChars;

        [Header("Sound")]
        public string soundName;
        public SoundType soundGenerationType;

        [Header("Effecs")]
        public int effectDistance;

        public void Clone(DialogueData data)
        { 
            fontSize = data.fontSize;
            color = data.color;
            style = data.style;
            characterSpacing = data.characterSpacing;
            wordSpacing = data.wordSpacing;
            colorGradient = data.colorGradient;

            timeBetweenChars = data.timeBetweenChars;
            effectDistance = data.effectDistance;
            soundName = data.soundName;
            soundGenerationType = data.soundGenerationType;
        }

        public void SetTMP(ref TMP_Text tmp)
        {
            tmp.fontSize = fontSize;
            tmp.fontStyle = style;
            tmp.color = color;
            tmp.characterSpacing = characterSpacing;
            tmp.wordSpacing = wordSpacing;

            if (colorGradient != null)
            {
                tmp.enableVertexGradient = true;
                tmp.colorGradientPreset = colorGradient;
            }
            else tmp.enableVertexGradient = false;
        }
    }
}
