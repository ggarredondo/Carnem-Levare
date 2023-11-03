using System.Collections;
using System.Collections.Generic;
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
        public List<string> soundsName;
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
            soundsName = new List<string>(data.soundsName);
            soundGenerationType = data.soundGenerationType;
        }
    }
}
