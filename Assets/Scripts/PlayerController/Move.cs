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
    /// <summary>
    /// The animation that the move performs.
    /// </summary>
    public Animation crouchLeftAnimation;
    public Animation leftAnimation;
    public Animation crouchRightAnimation;
    public Animation rightAnimation;

    [Header("Attack Values")]
    /// <summary>
    /// Type of move, divided by Light, Medium or Strong.
    /// </summary>
    public Type type;

    /// <summary>
    /// The damage it deals to the opponent's stamina, if it hits.
    /// </summary>
    public float damage;

    /// <summary>
    /// Speed of the animation.
    /// </summary>
    public float speed = 1f;

    /// <summary>
    /// Time before another move can be performed.
    /// </summary>
    public float cooldown;
}
