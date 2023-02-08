using UnityEngine;

public class Enemy : Character
{
    [Header("Movement Parameters")]
    [SerializeField] [Range(-1f, 1f)] private float horizontal = 0f;
    [SerializeField] [Range(-1f, 1f)] private float vertical = 0f;
    [SerializeField] private float enemyDirectionSpeed = 1f;
    [SerializeField] private bool block = false;

    private void DebugInput() {
        base.directionSpeed = enemyDirectionSpeed;
        base.isBlocking = block;
        direction.x = horizontal;
        direction.y = vertical;
    }

    override protected void Update()
    {
        SetAnimationParameters();

        DebugInput(); // DEBUG
    }
}
