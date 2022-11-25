using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelect : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Selectable>().Select();
    }
}
