using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy-specific Parameters")]
    [SerializeField] private float enemyDirectionSpeed = 1f;
    [SerializeField] private bool jab;

    [SerializeField] private bool block, lateBlock;
    [SerializeField] private float blockMaxTime = 5f;
    private float timer;

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
        if (isHurt || isBlocked) timer = 0f;
        Block((timer <= blockMaxTime && lateBlock) || block);
        timer += Time.deltaTime;
        AttackN(jab, 0);
    }
}
