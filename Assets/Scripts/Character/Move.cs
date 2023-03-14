using UnityEngine;

#region Enums
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
#endregion

/// <summary>
/// Specification of an attack performed by a player or NPC.
/// </summary>
public class Move : MonoBehaviour
{
    [SerializeField] private string moveName;

    [Header("Attack Animations")]
    // Animations that the move performs, depending on whether the Move slot is left or right, and if the player is currently crouching.
    public AnimationClip leftAnimation;
    [SerializeField] [Range(0f, 2f)] private float leftAnimationSpeed = 1f;

    public AnimationClip rightAnimation;
    [SerializeField] [Range(0f, 2f)] private float rightAnimationSpeed = 1f;

    [Header("Attack Sound")]
    [SerializeField] private string whiffSound;
    [SerializeField] private string hitSound;

    [Header("Attack Values")]
    [SerializeField] private HitboxType hitboxType;
    [Tooltip("States which type of hurt animation will play when it hits")] [SerializeField] private Power power;
    [SerializeField] private bool unblockable;
    [SerializeField] private float baseDamage; // Used to calculate damage dealt to the opponent's stamina, if it hits.

    #region TimeData
    [Header("Left Time Data (Normalized Animation Time)")]
    [Tooltip("[0, startup]: no hitbox. (startup, active]: hitbox is active.")]
    [SerializeField] [Range(0f, 1f)] private float leftStartUp = 0f;

    [Tooltip("(startup, active]: hitbox is active. (active, recovery]: no hitbox.")]
    [SerializeField] [Range(0f, 1f)] private float leftActive = 0f;

    [Tooltip("(active, recovery]: no hitbox. (recovery, 1]: can cancel animation.")]
    [SerializeField] [Range(0f, 1f)] private float leftRecovery = 0f;

    [Header("Right Time Data")]
    [Tooltip("[0, startup]: no hitbox. (startup, active]: hitbox is active.")]
    [SerializeField] [Range(0f, 1f)] private float rightStartUp = 0f;

    [Tooltip("(startup, active]: hitbox is active. (active, recovery]: no hitbox.")]
    [SerializeField] [Range(0f, 1f)] private float rightActive = 0f;

    [Tooltip("(active, recovery]: no hitbox. (recovery, 1]: can cancel animation.")]
    [SerializeField] [Range(0f, 1f)] private float rightRecovery = 0f;
    #endregion

    #region MovementValues
    [Header("(Extra) Movement Values")]

    [Tooltip("The attack may move the character further than established by root motion")]
    [SerializeField] private float leftMovement = 0f;

    [Tooltip("The attack may move the character further than established by root motion")]
    [SerializeField] private float rightMovement = 0f;
    #endregion

    #region ChargeValues
    [Header("Charge Values")]
    [Tooltip("Can it be charged? (right side only)")]
    [SerializeField] private bool chargeable = true;

    [System.NonSerialized] public bool pressed = false; // Check if this move specifically is held down.

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value)")]
    [SerializeField] [ConditionalField("chargeable")] private float chargeDecay; // Interpolation value used for lerp affecting chargeSpeed.

    [Tooltip("Minimum speed allowed when slowing down the attack animation")]
    [SerializeField] [ConditionalField("chargeable")] private float chargeMinSpeed = 0.01f;

    [Tooltip("Move will perform automatically after *chargeLimit* deltaTime seconds charging")]
    [SerializeField] [ConditionalField("chargeable")] private float chargeLimit = 2f;

    [Tooltip("The camera starts dolly zooming from a fraction of the chargeLimit value")]
    [SerializeField] [ConditionalField("chargeable")] private float chargeLimitDivisor = 6f;
    #endregion

    #region PublicMethods
    public string MoveName { get => moveName; }

    // Animation
    public float LeftAnimationSpeed { get => leftAnimationSpeed; }
    public float RightAnimationSpeed { get => rightAnimationSpeed; }

    // Sound
    public string WhiffSound { get => whiffSound; }
    public string HitSound { get => hitSound; }

    // Attack Values
    public HitboxType HitboxType { get => hitboxType; }
    public Power Power { get => power; }
    public bool Unblockable { get => unblockable; }
    public float BaseDamage { get => baseDamage; }

    // Charge Values
    public bool Chargeable { get => chargeable; }
    public float ChargeDecay { get => chargeDecay; }
    public float ChargeMinSpeed { get => chargeMinSpeed; }
    public float ChargeLimit { get => chargeLimit; }
    public float ChargeLimitDivisor { get => chargeLimitDivisor; }

    /// <summary>
    /// Move is active during the interval (startup, active].
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool isActive(Side side, float normalizedTime) {
        if (side == Side.Left)
            return normalizedTime > leftStartUp && normalizedTime <= leftActive;
        return normalizedTime > rightStartUp && normalizedTime <= rightActive;
    }

    /// <summary>
    /// Move can be cancelled when the animation normalized time is
    /// over *recovery* value.
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is greater than *recovery*, false otherwise</returns>
    public bool canCancel(Side side, float normalizedTime) {
        if (side == Side.Left) 
            return normalizedTime > leftRecovery;
        return normalizedTime > rightRecovery;
    }

    /// <summary>
    /// Returns the amount of extra movement the move may have depending on the side it was used from.
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <returns>Amount of extra movement</returns>
    public float getMovement(Side side) {
        if (side == Side.Left)
            return leftMovement;
        return rightMovement;
    }
    #endregion
}
