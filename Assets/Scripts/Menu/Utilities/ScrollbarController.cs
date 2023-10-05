using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollbarController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject parentRequired;
    [SerializeField] private RectTransform scanner;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private InputReader inputReader;

    private Vector2 direction;
    private Rect boundsRect;

    private void Awake()
    {
        direction = new Vector2(0, 0);
        boundsRect = GetWorldRect(scanner);

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

    private void MoveScrollbar(Vector2 direction)
    {
        float newPosition = scrollRect.verticalNormalizedPosition + direction.y * sensitivity * Time.deltaTime;
        newPosition = Mathf.Clamp(newPosition, -0.5f, 1.5f);
        scrollRect.verticalNormalizedPosition = newPosition;
    }

    private void AutomaticScrollbarMovement()
    {
        RectTransform currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();

        Rect objectRect = GetWorldRect(currentSelected);

        if (currentSelected.parent == parentRequired.transform)
        {
            if (objectRect.yMin < boundsRect.yMin && scrollRect.verticalNormalizedPosition > 0 && direction == new Vector2(0, 0))
                MoveScrollbar(new Vector2(0, -1));
            else if (objectRect.yMax > boundsRect.yMax && scrollRect.verticalNormalizedPosition < 1 && direction == new Vector2(0, 0))
                MoveScrollbar(new Vector2(0, 1));
            else
                MoveScrollbar(direction);
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
        if (GameManager.Input.ControlSchemeIndex == 0 || GameManager.Input.PreviousCustomControlScheme == InputDevice.KEYBOARD)
        {
            AutomaticScrollbarMovement();
        }
    }
}
