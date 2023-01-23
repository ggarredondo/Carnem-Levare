using UnityEngine;

// Used to choose between hurt animations, if it hits.
public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

// Part of limb used for hitting. Used to activate the corresponding hitbox.
// A hitbox must be added for each of these possible values for both player and enemy, for both left and right.
public enum Limb : uint
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
    [Header("Attack Animations")]
    // Animations that the move performs, depending on whether the Move slot is left or right, and if the player is currently crouching.
    public AnimationClip crouchLeftAnimation;
    public AnimationClip leftAnimation;
    public AnimationClip crouchRightAnimation;
    public AnimationClip rightAnimation;

    public float animationSpeed = 1f;

    [Header("Attack Values")]
    public Power power;
    public float damage; // Damage dealt to the opponent's stamina, if it hits.

    [Header("Hitbox Values")]
    [Range(0f, 1f)] public float startTime = 0f; // (Normalized) Time when hitbox is activated.
    [Range(0f, 1f)] public float endTime = 0f; // (Normalized) Time when hitbox is deactivated.
    public Limb limb;
}
