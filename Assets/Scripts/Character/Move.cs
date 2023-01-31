using UnityEngine;

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
    Elbow = 0,
    Fist = 1,
    Knee = 2,
    Shin = 3,
    Foot = 4
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

    [Header("Attack Values")]
    public Power power;
    public float damage; // Damage dealt to the opponent's stamina, if it hits.

    [Tooltip("Can you cancel the attack by blocking?")]
    public bool cancelable = true;

    [Header("Tracking Values")]
    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float commitStartTime = 0f;

    [Tooltip("Character stops tracking the opponent during the interval [commitStartTime, commitEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float commitEndTime = 0f;

    [System.NonSerialized] public bool pressed = false; // Used to check if the input is held down.
    [System.NonSerialized] public float chargeSpeed = 1f; // Attack animation modifier when input is held down.

    [Header("Charge Values")]
    [Tooltip("Can it be charged?")] 
    [SerializeField] private bool chargeable = true;

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value)")]
    public float leftChargeDecay = 1f, rightChargeDecay = 1f; // Interpolation value used for lerp affecting chargeSpeed.
    [System.NonSerialized] public float chargeDecay;

    [Tooltip("Move will perform automatically after *chargeLimit* frames charging")]
    [SerializeField] private int chargeLimit = 600;
    private int lastFrame;

    public enum ChargePhase { waiting, performing, canceled }
    private ChargePhase chargePhase;

    [Header("Hitbox Values")]
    public HitboxType hitboxType;
    [Tooltip("Hitbox is activated during the interval [hitboxStartTime, hitboxEndTime) of the normalized animation time")] 
    [SerializeField] [Range(0f, 1f)] private float hitboxStartTime = 0f, hitboxEndTime = 0f;

    /// <summary>
    /// Hitbox is active during the interval [hitboxStartTime, hitboxEndTime).
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <returns></returns>
    public bool isHitboxActive(float normalizedTime) {
        return normalizedTime >= hitboxStartTime && normalizedTime < hitboxEndTime;
    }

    /// <summary>
    /// Character stops tracking the opponent during the interval [commitStartTime, commitEndTime)
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    public bool isTracking(float normalizedTime) {
        return !(normalizedTime >= commitStartTime && normalizedTime < commitEndTime);
    }

    /// <summary>
    /// Slows down attack animation if attack button is held down, until it's released or 
    /// the animation speed reaches a minimum.
    /// </summary>
    /// <param name="attackSpeed">Character's attack speed</param>
    public void ChargeAttack(bool inTransition, float attackSpeed)
    {
        switch (chargePhase)
        {
            case ChargePhase.waiting:
                if (pressed && chargeable) {
                    chargePhase = ChargePhase.performing;
                    lastFrame = Time.frameCount;
                }
                break;

            case ChargePhase.performing:
                if (pressed && !inTransition)
                    chargeSpeed = Mathf.Lerp(chargeSpeed, 0f, chargeDecay * attackSpeed * Time.deltaTime);

                if (!pressed || Time.frameCount >= lastFrame + chargeLimit) {
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

    public int getLastFrame { get { return lastFrame; } }
}
