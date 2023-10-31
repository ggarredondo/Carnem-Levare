using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextGenerator : MonoBehaviour
{
    [System.Serializable]
    private struct SpecialCharacter
    {
        public char character;
        [Range(-100f,1000f)] public float addition;
    }

    [Header("Requirements")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Text textBoxDuplicate;
    [SerializeField] private DialogueTree dialogueTree;

    [Header("Parameters")]
    [SerializeField] [Range(1f,2f)] private float aceleration;
    [SerializeField] private List<SpecialCharacter> specialChars;
    [SerializeField] private bool repeatLastLine;

    private RNG random;
    private Dictionary<char, float> specialCharsDictionary;
    private Coroutine typing;
    private float speedMultiplier;
    private SoundType soundType;

    private void Awake()
    {
        speedMultiplier = 1;

        random = new();
        specialCharsDictionary = new();

        foreach (SpecialCharacter special in specialChars) {
            specialCharsDictionary.Add(special.character, special.addition / 100f);
        }

        dialogueTree.Initialize();
    }

    private void GenerateText()
    {
        Configure(dialogueTree.CurrentLine);
        typing = StartCoroutine(TypeLine(dialogueTree.CurrentLine));
    }

    public void NextLine()
    {
        if (typing == null)
        {
            if (dialogueTree.Next() || repeatLastLine)
            {
                ResetText();
                GenerateText();
            }
        }
        else speedMultiplier = Mathf.Clamp(speedMultiplier * aceleration, 1, 2);
    }

    public void PreviousLine()
    {
        if (typing == null)
        {
            if (dialogueTree.Previous() || repeatLastLine)
            {
                ResetText();
                GenerateText();
            }
        }
        else speedMultiplier = Mathf.Clamp(speedMultiplier * aceleration, 1, 2);
    }

    private void Configure(IHaveText line)
    {
        soundType = line.SoundGenerationType;

        textBox.fontSize = line.FontSize;
        textBoxDuplicate.fontSize = line.FontSize;

        textBox.fontStyle = line.Style;
        textBoxDuplicate.fontStyle = line.Style;

        textBox.color = line.Color;
        textBoxDuplicate.color = line.Color;

        textBox.characterSpacing = line.CharacterSpacing;
        textBoxDuplicate.characterSpacing = line.CharacterSpacing;

        textBox.wordSpacing = line.WordSpacing;
        textBoxDuplicate.wordSpacing = line.WordSpacing;

        if (line.ColorGradient != null)
        {
            textBox.enableVertexGradient = true;
            textBoxDuplicate.enableVertexGradient = true;

            textBox.colorGradientPreset = line.ColorGradient;
            textBoxDuplicate.colorGradientPreset = line.ColorGradient;
        }
        else
        {
            textBox.enableVertexGradient = false;
            textBoxDuplicate.enableVertexGradient = false;
        }
    }

    private IEnumerator TypeLine(IHaveText line)
    {
        var text = line.Text;
        var effectDistance = line.EffectDistance;

        if (soundType == SoundType.BY_LINE)
            PlaySound(ref line);

        for (int i = 0; i < text.Length + effectDistance; i++)
        {
            if (i < text.Length)
                textBoxDuplicate.text += text[i];

            if (i >= effectDistance)
            {
                var effectDelayChar = text[i - effectDistance];
                textBox.text += effectDelayChar;

                SelectSound(ref effectDelayChar, ref line);

                yield return StartCoroutine(WaitTime(effectDelayChar, line));
            }
        }

        typing = null;
        speedMultiplier = 1;
    }

    private IEnumerator WaitTime(char letter, IHaveText line)
    {
        if (specialCharsDictionary.ContainsKey(letter))
        {
            float newTime = Mathf.Clamp(line.TimeBetweenChars + line.TimeBetweenChars * specialCharsDictionary[letter], 0f, 5f);
            yield return new WaitForSecondsRealtime(newTime / speedMultiplier);
        }
        else
        {
            yield return new WaitForSecondsRealtime(line.TimeBetweenChars / speedMultiplier);
        }
    }

    private void SelectSound(ref char letter, ref IHaveText line)
    {
        if (soundType == SoundType.BY_CHARACTER)
            PlaySound(ref line);

        if (soundType == SoundType.BY_WORD && letter == ' ')
            PlaySound(ref line);
    }

    private void PlaySound(ref IHaveText line)
    {
        if (line.SoundsName.Count > 1)
            GameManager.Audio.Play(line.SoundsName[random.RangeInt(0, line.SoundsName.Count - 1)]);
        else
            GameManager.Audio.Play(line.SoundsName[0]);
    }

    private void ResetText()
    {
        textBox.text = "";
        textBoxDuplicate.text = "";
    }
}

public enum SoundType
{
    BY_CHARACTER,
    BY_WORD,
    BY_LINE
}
