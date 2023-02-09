using UnityEngine;

public enum ChargePhase { waiting, performing, canceled }

public class PlayerAttackManager : AttackManager
{
    private ChargePhase chargePhase = ChargePhase.waiting;
    private float deltaTimer;
    private CameraEffects cameraEffect;

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cameraEffect = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraEffects>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // Assign charge attack timings to camera.
        cameraEffect.SetChargeValues(chargePhase, deltaTimer, currentMove.getChargeLimit, currentMove.getChargeLimitDivisor);
        cameraEffect.Initialized();
    }

    /// <summary>
    /// Slows down attack animation if attack button is held down, until it's released or 
    /// the animation speed reaches a minimum. Only attacks coming from the right, only player.
    /// </summary>
    private void ChargeAttack(Animator animator, int layerIndex)
    {
        switch (chargePhase)
        {
            case ChargePhase.waiting:
                if (currentMove.pressed && currentMove.getChargeable && !animator.IsInTransition(layerIndex))
                {
                    deltaTimer = 0f;
                    chargePhase = ChargePhase.performing;
                }
                break;

            case ChargePhase.performing:
                if (!currentMove.pressed || deltaTimer >= currentMove.getChargeLimit)
                {
                    animator.speed = 1f;
                    chargePhase = ChargePhase.canceled;
                }
                else if (currentMove.pressed)
                {
                    animator.speed = Mathf.Lerp(animator.speed, 0f, currentMove.getChargeDecay * Time.deltaTime);
                    deltaTimer += Time.deltaTime;
                }
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (side == Side.Right) ChargeAttack(animator, layerIndex);
        cameraEffect.SetChargeValues(chargePhase, deltaTimer); // Camera checks Charge timings every update.
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        chargePhase = ChargePhase.waiting;
    }
}
