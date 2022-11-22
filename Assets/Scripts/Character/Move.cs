using UnityEngine;

public enum Power: uint
{
    Light = 0,
    Medium = 1,
    Strong = 2
}

public enum Direction: int
{
    Left = -2,
    Descending = -1,
    Ascending = 1,
    Right = 2
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
    /// Move's direction. Whether it's coming from the Left, the middle or the Right. And if it's coming from the middle, 
    /// whether it's Descending or Ascending.
    /// </summary>
    public Direction direction;

    /// <summary>
    /// The damage it deals to the opponent's stamina, if it hits.
    /// </summary>
    public float damage;

    /// <summary>
    /// Assign left or right to horizontal moves. Only works if the move isn't descending or ascending.
    /// </summary>
    /// <param name="dir">Left/Right direction.</param>
    public void LeftOrRight(Direction dir)
    {
        if (direction != Direction.Descending && direction != Direction.Ascending)
            direction = dir;
    }
}
