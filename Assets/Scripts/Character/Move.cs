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

    [Header("Charge Values")]
    [System.NonSerialized] public bool pressed = false; // Used to track if the input is held down.
    [System.NonSerialized] public float chargeSpeed = 1f; // Used when input is held down.
    [Tooltip("Can it be charged?")] [SerializeField] private bool chargeable = true;
    [Tooltip("How quickly the animation slows down when holding the attack button (interpolation value)")]
    [SerializeField] private float chargeDecay = 1f; // Interpolation value used for lerp affecting chargeSpeed.
    [Tooltip("Charging attacks is only allowed during the interval [chargeStartTime, chargeEndTime) of the normalized animation time")]
    [SerializeField] [Range(0f, 1f)] private float chargeStartTime = 0f, chargeEndTime = 0f;

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
    /// Slows down attack animation if attack button is held down during the interval [chargeStartTime, chargeEndTime)
    /// </summary>
    /// <param name="normalizedTime">Normalized time of the animation</param>
    /// <param name="attackSpeed">Character's attack speed</param>
    public void ChargeAttack(float normalizedTime, float attackSpeed)
    {
        bool withinInterval = normalizedTime >= chargeStartTime && normalizedTime < chargeEndTime;
        if (chargeable && pressed && withinInterval)
            chargeSpeed = Mathf.Lerp(chargeSpeed, 0f, chargeDecay * attackSpeed * Time.deltaTime);
        else
            chargeSpeed = 1f;
    }
}
