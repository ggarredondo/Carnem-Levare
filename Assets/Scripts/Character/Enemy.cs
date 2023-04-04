using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy-specific Parameters")]
    [SerializeField] private float enemyDirectionSpeed = 1f;
    [SerializeField] private bool singleJab, constantJab;

    [SerializeField] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] private bool block, lateBlock;
    [SerializeField] private float blockMaxTime = 5f;
    private float blockTimer;

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
        directionTarget.x = horizontal;
        directionTarget.y = vertical;

        if (state == CharacterState.hurt || state == CharacterState.blocked) blockTimer = 0f;
        Block((blockTimer <= blockMaxTime && lateBlock) || block);
        blockTimer += Time.deltaTime;
        AttackN(singleJab || constantJab, 0);
        singleJab = false;
    }
}
