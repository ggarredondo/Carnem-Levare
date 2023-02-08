using UnityEngine;

// Does the attack come from the sides or is it straight?
// Used to choose between hurt animations, if it hits.
public enum Direction
{
    Curved,
    Straight
}

// Used to choose between hurt animations, if it hits.
public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

// Used to choose the corresponding hitbox from a list of hitboxes (left list and right list).
// A hitbox must be added for each of these possible values for both player and enemy, for both left and right.
public enum HitboxType : uint
{
    Fist = 0
}

// Used by Attack Managers to choose from which side the attack is coming from
public enum Side : int
{
    Left = -1,
    Right = 1
}

/// <summary>
/// Specification of an attack performed by a player or NPC.
/// </summary>
public class Move : MonoBehaviour
{
    public string moveName;

    [Header("Attack Animations")]
    // Animations that the move performs, depending on whether the Move slot is left or right, and if the player is currently crouching.
    public AnimationClip leftAnimation;
    [SerializeField] [Range(0f, 2f)] private float leftAnimationSpeed = 1f;

    public AnimationClip rightAnimation;
    [SerializeField] [Range(0f, 2f)] private float rightAnimationSpeed = 1f;
    private float currentRightAnimationSpeed;

    [Header("Attack Values")]
    public Direction direction;
    public Power power;
    public float baseDamage; // Used to calculate damage dealt to the opponent's stamina, if it hits.

    [Header("Tracking Values")]
    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitStartTime = 0f;

    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitEndTime = 0f, rightCommitStartTime = 0f, rightCommitEndTime = 0f;

    [Header("Charge Values")]
    [Tooltip("Can it be charged?")]
    [SerializeField] private bool chargeable = true;

    [System.NonSerialized] public bool pressed = false; // Used to check if input is held down.

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value) (right side only)")]
    [SerializeField] private float chargeDecay; // Interpolation value used for lerp affecting chargeSpeed.

    [Tooltip("Move will perform automatically after *chargeLimit* deltaTime seconds charging")]
    [SerializeField] private float chargeLimit = 2f;
    private float deltaTimer = 0f;

    [Tooltip("The camera starts dolly zooming from a fraction of the chargeLimit value")]
    public float chargeLimitDivisor = 6f;

    public enum ChargePhase { waiting, performing, canceled }
    private ChargePhase chargePhase;

    [Header("Hitbox Values")]
    public HitboxType hitboxType;
    [Tooltip("Hitbox is activated during the interval [hitboxStartTime, hitboxEndTime) of the normalized animation time")] 
    [SerializeField] [Range(0f, 1f)] private float leftHitboxStartTime = 0f, leftHitboxEndTime = 0f;

    [Tooltip("Hitbox is activated during the interval [hitboxStartTime, hitboxEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float rightHitboxStartTime = 0f, rightHitboxEndTime = 0f;

    /// <summary>
    /// Hitbox is active during the interval [hitboxStartTime, hitboxEndTime).
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns></returns>
    public bool isHitboxActive(Side side, float normalizedTime) {
        if (side == Side.Left)
            return normalizedTime >= leftHitboxStartTime && normalizedTime < leftHitboxEndTime;
        return normalizedTime >= rightHitboxStartTime && normalizedTime < rightHitboxEndTime;
    }

    /// <summary>
    /// Character stops tracking the opponent during the interval [commitStartTime, commitEndTime)
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    public bool isTracking(Side side, float normalizedTime) {
        if (side == Side.Left)
            return !(normalizedTime >= leftCommitStartTime && normalizedTime < leftCommitEndTime);
        return !(normalizedTime >= rightCommitStartTime && normalizedTime < rightCommitEndTime);
    }

    /// <summary>
    /// Slows down attack animation if attack button is held down, until it's released or 
    /// the animation speed reaches a minimum. Only attacks coming from the right.
    /// </summary>
    /// <param name="inTransition">Is the animator in transition?</param>
    public void ChargeAttack(bool inTransition)
    {
        switch (chargePhase)
        {
            case ChargePhase.waiting:
                if (pressed && chargeable) {
                    currentRightAnimationSpeed = rightAnimationSpeed;
                    chargePhase = ChargePhase.performing;
                    deltaTimer = 0f;
                }
                break;

            case ChargePhase.performing:
                if (pressed && !inTransition) {
                    currentRightAnimationSpeed = Mathf.Lerp(currentRightAnimationSpeed, 0f, chargeDecay * rightAnimationSpeed * Time.deltaTime);
                    deltaTimer += Time.deltaTime;
                }

                if (!pressed || deltaTimer >= chargeLimit) {
                    currentRightAnimationSpeed = rightAnimationSpeed;
                    chargePhase = ChargePhase.canceled;
                }
                break;
        }
    }

    /// <summary>
    /// Resets move's chargePhase to waiting state.
    /// </summary>
    public void ResetChargePhase() { chargePhase = ChargePhase.waiting; }

    public float getLeftAnimationSpeed { get { return leftAnimationSpeed; } }
    public float getRightAnimationSpeed { get { return currentRightAnimationSpeed; } }

    public ChargePhase getChargePhase { get { return chargePhase; } }
    public float getDeltaTimer { get { return deltaTimer; } }
    public float getChargeLimit { get { return chargeLimit; } }
}
