using UnityEngine;

#region Enums
// Used to choose between hurt animations, if it hits.
public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

// Used by Attack Manager to know from which side the attack is coming from
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
    [SerializeField] private AnimationClip leftAnimation;
    [SerializeField] [Range(0f, 2f)] private float leftAnimationSpeed = 1f;

    [SerializeField] private AnimationClip rightAnimation;
    [SerializeField] [Range(0f, 2f)] private float rightAnimationSpeed = 1f;

    [Header("Attack Sound")]
    [SerializeField] private string whiffSound;
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack Values")]
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

    #region PublicMethods
    public string MoveName { get => moveName; }

    // Animation
    public AnimationClip LeftAnimation { get => leftAnimation; }
    public float LeftAnimationSpeed { get => leftAnimationSpeed; }
    public AnimationClip RightAnimation { get => rightAnimation; }
    public float RightAnimationSpeed { get => rightAnimationSpeed; }

    // Sound
    public string WhiffSound { get => whiffSound; }
    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }

    // Attack Values
    public Power Power { get => power; }
    public bool Unblockable { get => unblockable; }
    public float BaseDamage { get => baseDamage; }

    /// <summary>
    /// Move is active during the interval (startup, active].
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool IsActive(Side side, float normalizedTime) {
        if (side == Side.Left)
            return normalizedTime > leftStartUp && normalizedTime <= leftActive;
        return normalizedTime > rightStartUp && normalizedTime <= rightActive;
    }

    /// <summary>
    /// Move is recovering during the interval (active, recovery].
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool IsRecovering(Side side, float normalizedTime) {
        if (side == Side.Left)
            return normalizedTime > leftActive && normalizedTime <= leftRecovery;
        return normalizedTime > rightActive && normalizedTime <= rightRecovery;
    }

    /// <summary>
    /// Move can be cancelled when the animation normalized time is
    /// over *recovery* value.
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is greater than *recovery*, false otherwise</returns>
    public bool HasRecovered(Side side, float normalizedTime) {
        if (side == Side.Left) 
            return normalizedTime > leftRecovery;
        return normalizedTime > rightRecovery;
    }

    /// <summary>
    /// Returns the amount of extra movement the move may have depending on the side it was used from.
    /// </summary>
    /// <param name="side">Which side is the attack coming from?</param>
    /// <returns>Amount of extra movement</returns>
    public float GetMovement(Side side) {
        if (side == Side.Left)
            return leftMovement;
        return rightMovement;
    }
    #endregion
}
