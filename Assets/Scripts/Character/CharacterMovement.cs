using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Transform target;
    public void SetTarget(Transform target) => this.target = target;

    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;

    [SerializeField] private bool doTracking = true;

    private Rigidbody rb;
    private Vector2 direction;

    [SerializeField] private float directionSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        direction = Vector2.zero;
    }

    public void MoveCharacter(Vector2 directionTarget)
    {
        direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);
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
