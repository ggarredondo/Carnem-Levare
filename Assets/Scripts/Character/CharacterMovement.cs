using UnityEngine;
using RefDelegates;
using System.Linq;

[System.Serializable]
public class CharacterMovement
{
    private Rigidbody rb;
    private Transform character, target;
    public void SetTarget(in Transform target) => this.target = target;
    
    [Header("Tracking")]
    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;
    [SerializeField] private float targetingMinimumMagnitude = 0.1f;
    [SerializeField] private bool doTracking = true;

    [Header("Direction Speeds")]
    [Tooltip("How quickly the character's direction will match the target direction (smoothing movement)")]
    [SerializeField] private float walkingDirectionSpeed = 1f;
    [SerializeField] private float blockingDirectionSpeed = 1f, blockedDirectionSpeed = 1f, idleDirectionSpeed = 1f;
    
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
    public void MoveCharacter(in Vector2 targetDirection, float directionSpeed)
    {
        float finalDirectionSpeed = targetDirection.magnitude > 0f ? directionSpeed : idleDirectionSpeed;
        direction = Vector2.Lerp(direction, targetDirection, finalDirectionSpeed * Time.deltaTime);
        OnMoveCharacter?.Invoke(direction);
    }

    /// <summary>
    /// Physics-related. Use in Fixed Update.
    /// </summary>
    public void PushCharacter(in Vector3 knockback)
    {
        Vector3 knockbackDirection = target.right * knockback.x + target.up * knockback.y + target.forward * knockback.z;
        rb.AddForce(knockbackDirection, ForceMode.Impulse);
    }

    /// <summary>
    /// Physics-related. Use in Fixed Update.
    /// </summary>
    public void LookAtTarget(params bool[] conditions)
    {
        Quaternion targetRotation;
        if (target != null && doTracking && conditions.All(b => b))
        {
            targetRotation = Quaternion.Euler(0f, Quaternion.LookRotation(target.position - character.position).normalized.eulerAngles.y, 0f);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, trackingRate * Time.fixedDeltaTime);
        }
    }

    public bool IsIdle => (direction.magnitude <= targetingMinimumMagnitude);
    public ref readonly float WalkingDirectionSpeed => ref walkingDirectionSpeed;
    public ref readonly float BlockingDirectionSpeed => ref blockingDirectionSpeed;
    public ref readonly float BlockedDirectionSpeed => ref blockedDirectionSpeed;
}
