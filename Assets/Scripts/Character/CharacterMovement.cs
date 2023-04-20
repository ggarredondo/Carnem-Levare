using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform target;
    public void SetTarget(Transform target) => this.target = target;
    
    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;

    [SerializeField] private bool doTracking = true;

    [Tooltip("How quickly the character's direction will match the target direction (smoothing purposes)")]
    [SerializeField] private float directionSpeed = 1f;
    private Vector2 direction;
    public UnityAction<Vector2> OnMoveCharacter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        direction = Vector2.zero;
    }

    public void MoveCharacter(Vector2 targetDirection)
    {
        direction = Vector2.Lerp(direction, targetDirection, directionSpeed * Time.deltaTime);
        OnMoveCharacter.Invoke(direction);
    }

    public void LookAtTarget()
    {
        Quaternion targetRotation;
        if (target != null && doTracking)
        {
            targetRotation = Quaternion.LookRotation(target.position - transform.position);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, trackingRate * Time.fixedDeltaTime);
        }
    }
}
