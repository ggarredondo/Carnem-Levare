using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy-specific Parameters")]
    [SerializeField] private float enemyDirectionSpeed = 1f;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        base.Start();
    }
    protected override void Update()
    {
        directionSpeed = enemyDirectionSpeed;
        base.Update();
    }
}
