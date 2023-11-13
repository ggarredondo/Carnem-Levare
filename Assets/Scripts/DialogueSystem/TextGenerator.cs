using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DialogueTreeUtilities;

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
    [SerializeField] private List<SpecialCharacter> specialChars;
    [SerializeField] private bool repeatLastLine;

    private Dictionary<char, float> specialCharsDictionary;
    private Coroutine typing;
    private float speedMultiplier;
    private SoundType soundType;

    private void Awake()
    {
        speedMultiplier = 1;

        specialCharsDictionary = new();
        foreach (SpecialCharacter special in specialChars) {
            specialCharsDictionary.Add(special.character, special.addition / 100f);
        }

        dialogueTree.Initialize();
    }

    private void GenerateText()
    {
        Configure(dialogueTree.CurrentData);
        typing = StartCoroutine(TypeLine(dialogueTree.CurrentData, dialogueTree.CurrentText));
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
        else speedMultiplier = 0;
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
        else speedMultiplier = 0;
    }

    private void Configure(DialogueData line)
    {
        soundType = line.soundGenerationType;

        line.SetTMP(ref textBox);
        line.SetTMP(ref textBoxDuplicate);
    }

    private IEnumerator TypeLine(DialogueData currentData, string currentText)
    {
        var text = currentText;
        var effectDistance = currentData.effectDistance;

        if (soundType == SoundType.BY_LINE)
            PlaySound(ref currentData);

        for (int i = 0; i < text.Length + effectDistance; i++)
        {
            if (i < text.Length)
            {
                textBoxDuplicate.text += text[i];
            }

            if (i >= effectDistance)
            {
                var effectDelayChar = text[i - effectDistance];

                textBox.text += effectDelayChar;
                SelectSound(ref effectDelayChar, ref currentData);
                yield return StartCoroutine(WaitTime(effectDelayChar, currentData));
            }
        }

        typing = null;
        speedMultiplier = 1;
    }

    private IEnumerator WaitTime(char letter, DialogueData line)
    {
        if (specialCharsDictionary.ContainsKey(letter))
        {
            float newTime = Mathf.Clamp(line.timeBetweenChars + line.timeBetweenChars * specialCharsDictionary[letter], 0f, 5f);
            yield return new WaitForSecondsRealtime(newTime * speedMultiplier);
        }
        else yield return new WaitForSecondsRealtime(line.timeBetweenChars * speedMultiplier);
    }

    private void SelectSound(ref char letter, ref DialogueData line)
    {
        if (soundType == SoundType.BY_CHARACTER)
            PlaySound(ref line);

        if (soundType == SoundType.BY_WORD && letter == ' ')
            PlaySound(ref line);
    }

    private void PlaySound(ref DialogueData line)
    {
        GameManager.Audio.Play(line.soundName);
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
