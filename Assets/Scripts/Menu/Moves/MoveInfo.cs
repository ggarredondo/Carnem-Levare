using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using LerpUtilities;

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
        await Lerp.RectTransform_Color(rectTransform,
                                       image,
                                       new Vector3(rectTransform.localPosition.x,
                                                   rectTransform.localPosition.y + yPositionDifference, 
                                                   rectTransform.localPosition.z),
                                       new Color(color.r,color.g,color.b, initialAlpha/2),
                                       lerpDuration);

        await Lerp.RectTransform_Color(rectTransform,
                                       image,
                                       new Vector3(rectTransform.localPosition.x,
                                                   rectTransform.localPosition.y - yPositionDifference,
                                                   rectTransform.localPosition.z),
                                       new Color(1, 1, 1, initialAlpha),
                                       lerpDuration);
        isMoving = false;
    }
}
