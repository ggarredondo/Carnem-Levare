using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextGenerator : MonoBehaviour
{
    private enum SoundType
    {
        BY_CHARACTER,
        BY_WORD
    }

    [System.Serializable]
    private struct SpecialCharacter
    {
        public char character;
        [Range(-100f,1000f)] public float addition;
    }

    [Header("Requirements")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Text textBoxDuplicate;
    [SerializeField] private TMP_InputField inputField;

    [Header("Parameters")]
    [SerializeField] [Range(0f,0.2f)]private float timeBetweenChars;
    [SerializeField] private List<string> soundsName;
    [SerializeField] private SoundType soundType;
    [SerializeField] private List<SpecialCharacter> specialChars;
    [SerializeField] private int effectDistance;

    private string inputText;
    private RNG random;
    private Dictionary<char, float> specialCharsDictionary;

    private void Awake()
    {
        random = new();
        specialCharsDictionary = new();

        foreach (SpecialCharacter special in specialChars) {
            specialCharsDictionary.Add(special.character, special.addition / 100f);
        }
    }

    public void GenerateText()
    {
        inputText = inputField.text;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        for (int i = 0; i < inputText.Length + effectDistance; i++)
        {
            if(i < inputText.Length)
                textBoxDuplicate.text += inputText[i];

            if (i - effectDistance >= 0)
            {
                char effectDelayChar = inputText[i - effectDistance];
                textBox.text += effectDelayChar;

                if (soundType == SoundType.BY_CHARACTER)
                    PlaySound();

                if (specialCharsDictionary.ContainsKey(effectDelayChar))
                {
                    float newTime = Mathf.Clamp(timeBetweenChars + timeBetweenChars * specialCharsDictionary[effectDelayChar], 0f, 5f);
                    yield return new WaitForSecondsRealtime(newTime);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(timeBetweenChars);
                }

                if (soundType == SoundType.BY_WORD && effectDelayChar == ' ')
                    PlaySound();
            }
        }
    }

    private void PlaySound()
    {
        if (soundsName.Count > 1)
            GameManager.Audio.Play(soundsName[random.RangeInt(0, soundsName.Count - 1)]);
        else
            GameManager.Audio.Play(soundsName[0]);
    }
}
