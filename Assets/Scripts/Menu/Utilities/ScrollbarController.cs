using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject parentRequired;
    [SerializeField] private RectTransform scanner;
    [SerializeField] private RectTransform content;
    [SerializeField] private InputReader inputReader;

    [Header("Parameters")]
    [SerializeField] private float joystickSensitivity;
    [SerializeField] private float automaticSensitivity;
    [SerializeField] [Range(-0.5f, 0)] private float clampMinValue;
    [SerializeField] [Range(1, 1.5f)] private float clampMaxValue;

    private Vector2 direction;
    private Rect boundsRect, contentRect, objectRect;
    private float maxDistance;
    private GameObject currentSelected;

    private void Awake()
    {
        inputReader.ScrollbarEvent += UpdateDirection;
        UpdateRects();
    }

    private void OnDestroy()
    {
        inputReader.ScrollbarEvent -= UpdateDirection;
    }

    private void UpdateDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void MoveScrollbar(Vector2 dir, float sensitivity)
    {
        float newPosition = scrollRect.verticalNormalizedPosition + dir.y * sensitivity * Time.unscaledDeltaTime;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(newPosition, clampMinValue, clampMaxValue);
    }

    private void UpdateRects()
    {
        Rect newBoundsRect = GetWorldRect(scanner);
        contentRect = GetWorldRect(content);

        if(currentSelected != null)
            objectRect = GetWorldRect(currentSelected.GetComponent<RectTransform>());

        if (newBoundsRect != boundsRect)
        {
            maxDistance = Mathf.Abs(newBoundsRect.yMin - contentRect.yMin);
            boundsRect = newBoundsRect;
        }
    }

    private void AutomaticScrollbarMovement()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null) return;

        if (IsObjectAncestorOf(parentRequired, selected, 5)) currentSelected = selected;

        UpdateRects();

        float distance = 0;

        if (objectRect.yMin < boundsRect.yMin && scrollRect.verticalNormalizedPosition > 0 && direction == Vector2.zero)
        {
            distance = objectRect.yMin - boundsRect.yMin;
        }
        else if (objectRect.yMax > boundsRect.yMax && scrollRect.verticalNormalizedPosition < 1 && direction == Vector2.zero)
        {
            distance = objectRect.yMax - boundsRect.yMax;
        }

        if (distance != 0)
        {
            distance /= maxDistance;
            MoveScrollbar(new Vector2(0, distance), automaticSensitivity);
        }
        else
        {
            MoveScrollbar(direction, joystickSensitivity);
        }
    }

    bool IsObjectAncestorOf(GameObject potentialAncestor, GameObject child, int maxDepth)
    {
        if (maxDepth < 0 || child == null)
        {
            return false;
        }

        if (child.transform.parent == potentialAncestor.transform)
        {
            return true;
        }

        if (child.transform.parent == null)
        {
            return false;
        }

        return IsObjectAncestorOf(potentialAncestor, child.transform.parent.gameObject, maxDepth - 1);
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0], corners[2] - corners[0]);
    }

    private void Update()
    {
        if(GameManager.Input.NeedToSelect)
            AutomaticScrollbarMovement();
    }
}
