using UnityEngine;

public enum Type
{
    Light,
    Medium,
    Strong
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
    /// Type of move, classified by Light, Medium or Strong.
    /// </summary>
    public Type type;

    /// <summary>
    /// The damage it deals to the opponent's stamina, if it hits.
    /// </summary>
    public float damage;
}
