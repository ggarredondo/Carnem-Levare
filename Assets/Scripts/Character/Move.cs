using UnityEngine;

public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

public enum Side: int
{
    Left = -1,
    Center = 0,
    Right = 1
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
    /// <summary>
    /// Move's power, classified by Light, Medium and Strong. Used to choose between hurt animations.
    /// </summary>
    public Power power;

    /// <summary>
    /// Move's direction, from left to right (including center). Used to choose between hurt animations.
    /// </summary>
    public Side side;

    /// <summary>
    /// The damage it deals to the opponent's stamina, if it hits.
    /// </summary>
    public float damage;
}
