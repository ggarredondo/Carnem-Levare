using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using LerpUtilities;
using System.Threading.Tasks;

public class MoveInfo : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float yPositionDifference;
    [Range(0, 1)] [SerializeField] private float lerpDuration;

    [Header("Requirements")]
    [SerializeField] private TMP_Text names;
    [SerializeField] private TMP_Text data;

    private bool isMoving;

    private RectTransform rectTransform;
    private Image image;
    private float initialAlpha;

    public void Initialize(in List<string> list)
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        initialAlpha = image.color.a;
        UpdateInfoBox(in list);
    }

    public void UpdateInfoBox(in List<string> list)
    {
        if(!isMoving)
            Movement(new Color(1,1,1));

        UpdateText(in list);
    }

    private void UpdateText(in List<string> list)
    {
        names.text = "";
        data.text = "";

        for(int i = 0; i < list.Count; i += 2)
        {
            names.text += list[i] + "\n";
            data.text += list[i + 1] + "\n";
        }
    }

    public async void Movement(Color color)
    {
        isMoving = true;
        Vector3 newPosition = new(rectTransform.localPosition.x, rectTransform.localPosition.y + yPositionDifference, rectTransform.localPosition.z);

        Color newColor = new(color.r, color.g, color.b, initialAlpha / 2);

        await Task.WhenAll(Lerp.Value(rectTransform.localPosition, newPosition, (v) => rectTransform.localPosition = v, lerpDuration),
                           Lerp.Value(image.color, newColor, (c) => image.color = c, lerpDuration));

        newPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - yPositionDifference, rectTransform.localPosition.z);

        newColor = new Color(1, 1, 1, initialAlpha);

        await Task.WhenAll(Lerp.Value(rectTransform.localPosition, newPosition, (v) => rectTransform.localPosition = v, lerpDuration),
                           Lerp.Value(image.color, newColor, (c) => image.color = c, lerpDuration));

        isMoving = false;
    }
}
