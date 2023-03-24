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

    [Header("Time Data (ms)")]
    [Tooltip("Move is starting for *startUp* ms. No hitbox.")]
    [SerializeField] private float startUp = 0f;

    [Tooltip("Hitbox is active for *active* ms.")]
    [SerializeField] private float active = 0f;

    [Tooltip("Move is recovering for *recovery* ms.")]
    [SerializeField] private float recovery = 0f;

    [Tooltip("How many milliseconds the opponent takes to leave block animation.")]
    [SerializeField] private float advantageOnBlock = 0f;

    [Tooltip("How many milliseconds the opponent takes to leave hurt animation.")]
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

    public float StartUp { get => startUp; }
    public float Active { get => active; }
    public float Recovery { get => recovery; }

    public float AdvantageOnBlock { get => advantageOnBlock; }
    public float AdvantageOnHit { get => advantageOnHit; }

    public float ExtraMovement { get => extraMovement; }

    #endregion
}
