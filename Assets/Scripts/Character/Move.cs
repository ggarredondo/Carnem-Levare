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
    [Tooltip("Does the character track the opponent during the move?")] 
    [SerializeField] private bool track = false;
    [System.NonSerialized] public bool isTracking = false; // Nontracking moves may track regardless if they are being charged.

    [Tooltip("Can you cancel the attack by blocking?")]
    public bool cancelable = true;

    [System.NonSerialized] public bool pressed = false; // Used to check if the input is held down.
    [System.NonSerialized] public float chargeSpeed = 1f; // Attack animation modifier when input is held down.

    [Header("Charge Values")]
    [Tooltip("Can it be charged?")] 
    [SerializeField] private bool chargeable = true;

    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value)")]
    [SerializeField] private float chargeDecay = 1f; // Interpolation value used for lerp affecting chargeSpeed.

    [Tooltip("Move will perform automatically after *chargeLimit* frames charging")]
    [SerializeField] private int chargeLimit = 600;
    private int lastFrame;

    private enum ChargePhase { waiting, performing, canceled }
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
    public bool isHitboxActive(float normalizedTime)
    {
        return normalizedTime >= hitboxStartTime && normalizedTime < hitboxEndTime;
    }

    /// <summary>
    /// Slows down attack animation if attack button is held down, until it's released or 
    /// the animation speed reaches a minimum.
    /// </summary>
    /// <param name="attackSpeed">Character's attack speed</param>
    public void ChargeAttack(float attackSpeed)
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
                if (pressed) {
                    isTracking = true;
                    chargeSpeed = Mathf.Lerp(chargeSpeed, 0f, chargeDecay * attackSpeed * Time.deltaTime);
                }
                if (!pressed || Time.frameCount >= lastFrame + chargeLimit) {
                    isTracking = track;
                    chargeSpeed = 1f;
                    chargePhase = ChargePhase.canceled;
                }
                break;
        }
    }

    public void ResetChargePhase() { chargePhase = ChargePhase.waiting; }
}
