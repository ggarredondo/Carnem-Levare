using UnityEngine;

public class Player : Character
{
    [Header("Input Parameters")]

    [Tooltip("How quickly player animations follows stick movement")]
    [SerializeField] private float stickSpeed;

    [Tooltip("How quickly you can duck and sidestep when blocking")]
    [SerializeField] private float blockingStickSpeed;

    [Tooltip("How quickly you can duck and sidestep when an attack is blocked")]
    [SerializeField] private float blockedStickSpeed;

    [Tooltip("Lower stickSpeed to smooth out transitions to idle (when stick is centered)")]
    [SerializeField] private float smoothStickSpeed;

    private void OnEnable()
    {
        inputReader.MovementEvent += Movement;
        inputReader.BlockEvent += Block;

        inputReader.Left0Event += Left0;
        inputReader.Left1Event += Left1;

        inputReader.Right0Event += Right0;
        inputReader.Right1Event += Right1;
    }

    protected override void Start()
    {
        target = GameObject.FindWithTag("Enemy").transform;
        base.Start();
    }
    protected override void Update()
    {
        // Change movement animation blending speed depending on the situation.
        if (directionTarget.magnitude == 0f && !anim.GetCurrentAnimatorStateInfo(0).IsName("Block"))
            directionSpeed = smoothStickSpeed;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Block"))
            directionSpeed = blockingStickSpeed;
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Blocked"))
            directionSpeed = blockedStickSpeed;
        else
            directionSpeed = stickSpeed;

        base.Update();
    }

    private void OnDisable()
    {
        inputReader.MovementEvent -= Movement;
        inputReader.BlockEvent -= Block;

        inputReader.Left0Event -= Left0;
        inputReader.Left1Event -= Left1;

        inputReader.Right0Event -= Right0;
        inputReader.Right1Event -= Right1;
    }

    #region Actions

    private void Left0(bool performed) { LeftN(performed, 0); }
    private void Left1(bool performed) { LeftN(performed, 1); }

    private void Right0(bool performed) { RightN(performed, 0); }
    private void Right1(bool performed) { RightN(performed, 1); }

    #endregion
}
