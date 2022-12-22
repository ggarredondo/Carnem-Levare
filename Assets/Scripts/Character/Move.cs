using UnityEngine;

public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
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
    public Power power; // Used to choose between hurt animations, if it hits.
    public float damage; // Damage dealt to the opponent's stamina, if it hits.
}
