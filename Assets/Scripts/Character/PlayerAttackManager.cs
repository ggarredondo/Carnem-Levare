using UnityEngine;

public enum ChargePhase { waiting, performing, canceled }

public class PlayerAttackManager : StateMachineBehaviour
{
    private Player player;
    private Move currentMove;
    private bool currentMoveFound;
    private GameObject currentHitbox;
    private Side side;
    private ChargePhase chargePhase;
    private float deltaTimer;
    private CameraEffects cameraEffect;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cameraEffect = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraEffects>();
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
                if (currentMove.pressed && currentMove.getChargeable) {
                    deltaTimer = 0f;
                    chargePhase = ChargePhase.performing;
                }
                break;

            case ChargePhase.performing:
                if (currentMove.pressed && !animator.IsInTransition(layerIndex)) {
                    animator.speed = Mathf.Lerp(animator.speed, 0f, currentMove.getChargeDecay * Time.deltaTime);
                    deltaTimer += Time.deltaTime;
                }

                if (!currentMove.pressed || deltaTimer >= currentMove.getChargeLimit) {
                    animator.speed = 1f;
                    chargePhase = ChargePhase.canceled;
                }
                break;
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentMoveFound = false;
        for (int i = 0; i < player.getLeftMoveset.Count; ++i) {
            if (stateInfo.IsName("Left"+i)) {
                currentMove = player.getLeftMoveset[i];
                side = Side.Left;
                currentHitbox = player.leftHitboxes[(int)currentMove.hitboxType];
                currentMoveFound = true;
            }
        }
        for (int i = 0; i < player.getRightMoveset.Count && !currentMoveFound; ++i) {
            if (stateInfo.IsName("Right"+i)) {
                currentMove = player.getRightMoveset[i];
                side = Side.Right;
                currentHitbox = player.rightHitboxes[(int)currentMove.hitboxType];
            }
        }

        // Assigns the move's power and damage to the hitbox component so that once it hits the information is passed onto the hurtbox.
        currentHitbox.GetComponent<Hitbox>().power = currentMove.power;
        currentHitbox.GetComponent<Hitbox>().damage = player.CalculateAttackDamage(currentMove.baseDamage);
        // Assign the move's direction by checking if it's straight, and if it's not we assign left o right.
        currentHitbox.GetComponent<Hitbox>().side = currentMove.direction == Direction.Straight ? 0 : (int) side;

        // Assign charge attack timings to camera.
        cameraEffect.SetChargeValues(chargePhase, deltaTimer, currentMove.getChargeLimit, currentMove.getChargeLimitDivisor);
        cameraEffect.Initialized();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.tracking = currentMove.isTracking(side, stateInfo.normalizedTime);
        currentHitbox.GetComponent<Hitbox>().Activate(currentMove.isHitboxActive(side, stateInfo.normalizedTime));
        if (side == Side.Right) ChargeAttack(animator, layerIndex);

        // Camera checks Charge timings every update.
        cameraEffect.SetChargeValues(chargePhase, deltaTimer, currentMove.getChargeLimit, currentMove.getChargeLimitDivisor);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.tracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);
        chargePhase = ChargePhase.waiting;
    }

}
