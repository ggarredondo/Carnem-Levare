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
    [SerializeField] private DialogueTree dialogueTree;

    [Header("Parameters")]
    [SerializeField] [Range(1f,2f)] private float aceleration;
    [SerializeField] private SoundType soundType;
    [SerializeField] private List<SpecialCharacter> specialChars;

    private RNG random;
    private Dictionary<char, float> specialCharsDictionary;
    private Coroutine typing;
    private float speedMultiplier;

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

    private void Configure(IHaveText line)
    {
        textBox.fontSize = line.FontSize;
        textBoxDuplicate.fontSize = line.FontSize;
    }

    private IEnumerator TypeLine(IHaveText line)
    {
        for (int i = 0; i < line.Text.Length + line.EffectDistance; i++)
        {
            if(i < line.Text.Length)
                textBoxDuplicate.text += line.Text[i];

            if (i - line.EffectDistance >= 0)
            {
                char effectDelayChar = line.Text[i - line.EffectDistance];
                textBox.text += effectDelayChar;

                if (soundType == SoundType.BY_CHARACTER)
                    PlaySound(ref line);

                if (specialCharsDictionary.ContainsKey(effectDelayChar))
                {
                    float newTime = Mathf.Clamp(line.TimeBetweenChars + line.TimeBetweenChars * specialCharsDictionary[effectDelayChar], 0f, 5f);
                    yield return new WaitForSecondsRealtime(newTime / speedMultiplier);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(line.TimeBetweenChars / speedMultiplier);
                }

                if (soundType == SoundType.BY_WORD && effectDelayChar == ' ')
                    PlaySound(ref line);
            }
        }

        typing = null;
        speedMultiplier = 1;
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

    public void NextLine()
    {
        if (typing == null)
        {
            if (dialogueTree.Next())
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
            if (dialogueTree.Previous())
            {
                ResetText();
                GenerateText();
            }
        }
        else speedMultiplier = Mathf.Clamp(speedMultiplier * aceleration, 1, 2);
    }
}
