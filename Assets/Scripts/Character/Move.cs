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
    public float getLeftAnimationSpeed { get { return leftAnimationSpeed; } }

    public AnimationClip rightAnimation;
    [SerializeField] [Range(0f, 2f)] private float rightAnimationSpeed = 1f;
    public float getRightAnimationSpeed { get { return rightAnimationSpeed; } }

    [Header("Attack Values")]
    public Direction direction;
    public Power power;
    public float baseDamage; // Used to calculate damage dealt to the opponent's stamina, if it hits.

    #region TrackingValues
    [Header("Tracking Values")]
    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitStartTime = 0f;

    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float leftCommitEndTime = 0f, rightCommitStartTime = 0f, rightCommitEndTime = 0f;
    #endregion

    #region ChargeValues
    [Header("Charge Values")]
    [Tooltip("Can it be charged?")]
    [SerializeField] private bool chargeable = true;
    public bool getChargeable { get { return chargeable; } }

    [System.NonSerialized] public bool pressed = false; // Check if this move specifically is held down.

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value) (right side only)")]
    [SerializeField] private float chargeDecay; // Interpolation value used for lerp affecting chargeSpeed.
    public float getChargeDecay { get { return chargeDecay; } }

    [Tooltip("Move will perform automatically after *chargeLimit* deltaTime seconds charging")]
    [SerializeField] private float chargeLimit = 2f;
    public float getChargeLimit { get { return chargeLimit; } }

    [Tooltip("The camera starts dolly zooming from a fraction of the chargeLimit value")]
    [SerializeField] private float chargeLimitDivisor = 6f;
    public float getChargeLimitDivisor { get { return chargeLimitDivisor; } }

    [Header("Hitbox Values")]
    public HitboxType hitboxType;
    [Tooltip("Hitbox is activated during the interval [hitboxStartTime, hitboxEndTime) of the normalized animation time")] 
    [SerializeField] [Range(0f, 1f)] private float leftHitboxStartTime = 0f, leftHitboxEndTime = 0f;

    [Tooltip("Hitbox is activated during the interval [hitboxStartTime, hitboxEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float rightHitboxStartTime = 0f, rightHitboxEndTime = 0f;
    #endregion

    #region PublicMethods
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
    #endregion
}
