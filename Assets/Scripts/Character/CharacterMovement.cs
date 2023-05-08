using UnityEngine;
using RefDelegates;

[System.Serializable]
public class CharacterMovement
{
    private Rigidbody rb;
    private Transform character, target;
    public void SetTarget(in Transform target) => this.target = target;
    
    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;

    [SerializeField] private bool doTracking = true;

    [Tooltip("How quickly the character's direction will match the target direction (smoothing movement)")]
    [SerializeField] private float movingDirectionSpeed = 1f, idleDirectionSpeed = 1f;
    private const float IDLE_THRESHOLD = 0.01f;
    
    private Vector2 direction;
    public event ActionIn<Vector2> OnMoveCharacter;

    public void Initialize(in Transform character, in Transform target, in Rigidbody rb)
    { 
        this.character = character;
        this.target = target;
        this.rb = rb;
        direction = Vector2.zero;
    }

    /// <summary>
    /// Smooths move direction using Lerp and invokes OnMoveCharacter
    /// event with that smoothed direction as parameter.
    /// Doesn't do anything on its own.
    /// </summary>
    public void MoveCharacter(in Vector2 targetDirection)
    {
        float directionSpeed = targetDirection.magnitude > 0f ? movingDirectionSpeed : idleDirectionSpeed;
        direction = Vector2.Lerp(direction, targetDirection, directionSpeed * Time.deltaTime);
        OnMoveCharacter?.Invoke(direction);
    }

    /// <summary>
    /// Physics-related. Use in Fixed Update.
    /// </summary>
    public void LookAtTarget()
    {
        Quaternion targetRotation;
        if (target != null && doTracking && direction.magnitude > IDLE_THRESHOLD)
        {
            targetRotation = Quaternion.Euler(0f, Quaternion.LookRotation(target.position - character.position).normalized.eulerAngles.y, 0f);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, trackingRate * Time.fixedDeltaTime);
        }
    }
}
