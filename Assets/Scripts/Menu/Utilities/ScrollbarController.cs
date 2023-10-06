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
    private Rect boundsRect, contentRect;
    private float maxDistance;

    private void Awake()
    {
        direction = new Vector2(0, 0);
        boundsRect = GetWorldRect(scanner);
        contentRect = GetWorldRect(content);
        maxDistance = Mathf.Abs(boundsRect.yMin - contentRect.yMin);
        inputReader.ScrollbarEvent += UpdateDirection;
    }

    private void OnDestroy()
    {
        inputReader.ScrollbarEvent -= UpdateDirection;
    }

    private void UpdateDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    private void MoveScrollbar(Vector2 direction, float sensitivity)
    {
        float newPosition = scrollRect.verticalNormalizedPosition + direction.y * sensitivity * Time.deltaTime;
        newPosition = Mathf.Clamp(newPosition, clampMinValue, clampMaxValue);

        scrollRect.verticalNormalizedPosition = newPosition;
    }

    private void AutomaticScrollbarMovement()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null)
        {
            boundsRect = GetWorldRect(scanner);
            Rect objectRect = GetWorldRect(currentSelected.GetComponent<RectTransform>());

            if (currentSelected.transform.parent == parentRequired.transform)
            {
                if (objectRect.yMin < boundsRect.yMin && scrollRect.verticalNormalizedPosition > 0 && direction == new Vector2(0, 0))
                {
                    float distance = objectRect.yMin - boundsRect.yMin;
                    distance /= maxDistance;
                    MoveScrollbar(new Vector2(0, distance), automaticSensitivity);
                }
                else if (objectRect.yMax > boundsRect.yMax && scrollRect.verticalNormalizedPosition < 1 && direction == new Vector2(0, 0))
                {
                    float distance = objectRect.yMax - boundsRect.yMax;
                    distance /= maxDistance;
                    MoveScrollbar(new Vector2(0, distance), automaticSensitivity);
                }
                else
                    MoveScrollbar(direction, joystickSensitivity);
            }
        }
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0], corners[2] - corners[0]);
    }

    private void Update()
    {
        AutomaticScrollbarMovement();
    }
}
