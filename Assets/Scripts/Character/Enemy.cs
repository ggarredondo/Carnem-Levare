using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy-specific Parameters")]
    [SerializeField] private float enemyDirectionSpeed = 1f;
    [SerializeField] private bool jab;

    [SerializeField] private bool block;
    [SerializeField] private float blockMaxTime = 5f;
    private float deltaTimer;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        base.Start();
    }
    protected override void Update()
    {
        directionSpeed = enemyDirectionSpeed;
        Behaviour();
        base.Update();
    }

    private void Behaviour() {
        if (isHurt)
            deltaTimer = 0f;
        Block(deltaTimer <= blockMaxTime && block);
        deltaTimer += Time.deltaTime;
        LeftN(jab, 0);
    }
}
