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
    [Range(0f, 2f)] public float leftAnimationSpeed = 1f;
    public AnimationClip rightAnimation;
    [Range(0f, 2f)] public float rightAnimationSpeed = 1f;
    private float animationSpeed;

    [Header("Attack Values")]
    public Direction direction;
    public Power power;
    public float damage; // Damage dealt to the opponent's stamina, if it hits.

    [Tooltip("Can you cancel the attack by blocking?")]
    public bool cancelable = true;

    [Header("Tracking Values")]
    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitStartTime = 0f;

    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitEndTime = 0f, rightCommitStartTime = 0f, rightCommitEndTime = 0f;

    [System.NonSerialized] public bool pressed = false; // Used to check if the input is held down.
    [System.NonSerialized] public float chargeSpeed = 1f; // Attack animation modifier when input is held down.

    [Header("Charge Values")]
    [Tooltip("Can it be charged?")] 
    [SerializeField] private bool chargeable = true;

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value)")]
    public float leftChargeDecay = 1f, rightChargeDecay = 1f; // Interpolation value used for lerp affecting chargeSpeed.
    private float chargeDecay;

    [Tooltip("Move will perform automatically after *chargeLimit* deltaTime seconds charging")]
    [SerializeField] private float chargeLimit = 2f;
    private float deltaTimer = 0f;

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
    /// the animation speed reaches a minimum.
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="inTransition">Is the animator in transition?</param>
    /// <param name="attackSpeed">Character's attack speed</param>
    public void ChargeAttack(Side side, bool inTransition, float attackSpeed)
    {
        chargeDecay = side == Side.Left ? leftChargeDecay : rightChargeDecay;
        animationSpeed = side == Side.Left ? leftAnimationSpeed : rightAnimationSpeed;

        switch (chargePhase)
        {
            case ChargePhase.waiting:
                if (pressed && chargeable) {
                    chargePhase = ChargePhase.performing;
                    deltaTimer = 0f;
                }
                break;

            case ChargePhase.performing:
                if (pressed && !inTransition) {
                    chargeSpeed = Mathf.Lerp(chargeSpeed, 0f, chargeDecay * attackSpeed * animationSpeed * Time.deltaTime);
                    deltaTimer += Time.deltaTime;
                }

                if (!pressed || deltaTimer >= chargeLimit) {
                    chargeSpeed = 1f;
                    chargePhase = ChargePhase.canceled;
                }
                break;
        }
    }

    /// <summary>
    /// Resets move's chargePhase to waiting state.
    /// </summary>
    public void ResetChargePhase() { chargePhase = ChargePhase.waiting; }

    public ChargePhase getChargePhase { get { return chargePhase; } }

    public float getDeltaTimer { get { return deltaTimer; } }

    public float getChargeLimit { get { return chargeLimit; } }
}
