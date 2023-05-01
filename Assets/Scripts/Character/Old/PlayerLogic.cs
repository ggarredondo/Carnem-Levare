using UnityEngine;

public class PlayerLogic : CharacterLogic
{
    [Header("Player-specific Parameters")]

    [SerializeField] private InputReader inputReader;

    [Tooltip("How quickly player animations follows stick movement")]
    [SerializeField] private float stickSpeed;

    [Tooltip("How quickly you can duck and sidestep when blocking")]
    [SerializeField] private float blockingStickSpeed;

    [Tooltip("How quickly you can duck and sidestep when an attack is blocked")]
    [SerializeField] private float blockedStickSpeed;

    [Tooltip("Lower stickSpeed to smooth out transitions to idle (when stick is centered)")]
    [SerializeField] private float smoothStickSpeed;

    protected override void Start()
    {
        target = GameObject.FindWithTag("Enemy").transform;

        inputReader.MovementEvent += Movement;
        inputReader.BlockEvent += Block;

        inputReader.Attack0Event += Attack0;
        inputReader.Attack1Event += Attack1;
        inputReader.Attack2Event += Attack2;
        inputReader.Attack3Event += Attack3;

        base.Start();
    }
    protected override void Update()
    {
        // Change movement animation blending speed depending on the situation.
        if (directionTarget.magnitude == 0f && state == CharacterStateOld.WALKING)
            directionSpeed = smoothStickSpeed;
        else if (state == CharacterStateOld.BLOCKING)
            directionSpeed = blockingStickSpeed;
        else if (state == CharacterStateOld.BLOCKED)
            directionSpeed = blockedStickSpeed;
        else
            directionSpeed = stickSpeed;

        base.Update();
    }

    #region Actions

    private void Attack0(bool performed) { AttackN(performed, 0); }
    private void Attack1(bool performed) { AttackN(performed, 1); }

    private void Attack2(bool performed) { AttackN(performed, 2); }
    private void Attack3(bool performed) { AttackN(performed, 3); }

    #endregion
}
