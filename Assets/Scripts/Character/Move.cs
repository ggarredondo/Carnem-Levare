using UnityEngine;

// Used to choose between hurt animations, if it hits.
public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

[CreateAssetMenu(menuName = "Scriptable Objects/Move")]
public class Move : ScriptableObject
{
    [SerializeField] private string moveName;

    [Header("Attack Animations")]
    [SerializeField] private AnimationClip animation;
    [SerializeField] [Range(0f, 2f)] private float animationSpeed = 1f;
    [SerializeField] private string hitboxName;

    [Header("Attack Sound")]
    [SerializeField] private string whiffSound;
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack Values")]
    [Tooltip("States which type of hurt animation will play when it hits")] [SerializeField] private Power power;
    [SerializeField] private bool unblockable;
    [SerializeField] private float baseDamage; // Used to calculate damage dealt to the opponent's stamina, if it hits.

    [Tooltip("The attack may move the character further than established by root motion")]
    [SerializeField] private float extraMovement = 0f;

    #region TimeData

    [Header("Time Data (Normalized Animation Time)")]
    [Tooltip("[0, startup]: no hitbox. (startup, active]: hitbox is active.")]
    [SerializeField] [Range(0f, 1f)] private float startUp = 0f;

    [Tooltip("(startup, active]: hitbox is active. (active, recovery]: no hitbox.")]
    [SerializeField] [Range(0f, 1f)] private float active = 0f;

    [Tooltip("(active, recovery]: no hitbox. (recovery, 1]: can cancel animation.")]
    [SerializeField] [Range(0f, 1f)] private float recovery = 0f;

    [Tooltip("How many deltaseconds the opponent takes to leave block animation.")]
    [SerializeField] private float advantageOnBlock = 0f;

    [Tooltip("How many deltaseconds the opponent takes to leave hurt animation.")]
    [SerializeField] private float advantageOnHit = 0f;

    #endregion

    #region PublicMethods
    public string MoveName { get => moveName; }

    // Animation
    public AnimationClip Animation { get => animation; }
    public float AnimationSpeed { get => animationSpeed; }
    public string HitboxName { get => hitboxName; }

    // Sound
    public string WhiffSound { get => whiffSound; }
    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }

    // Attack Values
    public Power Power { get => power; }
    public bool Unblockable { get => unblockable; }
    public float BaseDamage { get => baseDamage; }

    /// <summary>
    /// Move is starting during the interval [0, startup].
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool IsStarting(float normalizedTime) { return normalizedTime < startUp; }

    /// <summary>
    /// Move is active during the interval (startup, active].
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool IsActive(float normalizedTime) {
        return normalizedTime > startUp && normalizedTime <= active;
    }

    /// <summary>
    /// Move is recovering during the interval (active, recovery].
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is in interval, false otherwise</returns>
    public bool IsRecovering(float normalizedTime) {
        return normalizedTime > active && normalizedTime <= recovery;
    }

    /// <summary>
    /// Move can be cancelled when the animation normalized time is
    /// over *recovery* value.
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns>True if time is greater than *recovery*, false otherwise</returns>
    public bool HasRecovered(float normalizedTime) {
        return normalizedTime > recovery;
    }

    public float AdvantageOnBlock { get => advantageOnBlock; }
    public float AdvantageOnHit { get => advantageOnHit; }

    public float ExtraMovement { get => extraMovement; }

    #endregion
}
