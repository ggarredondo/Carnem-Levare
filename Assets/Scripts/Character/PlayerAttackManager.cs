using UnityEngine;

public class PlayerAttackManager : StateMachineBehaviour
{
    private Player player;
    private Move currentMove;
    private bool currentMoveFound;
    private GameObject currentHitbox;
    private Side side;
    private CameraEffects cameraFollow;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cameraFollow = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraEffects>();
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

        cameraFollow.currentMove = currentMove;
        cameraFollow.Initialized();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.tracking = currentMove.isTracking(side, stateInfo.normalizedTime);
        currentHitbox.GetComponent<Hitbox>().Activate(currentMove.isHitboxActive(side, stateInfo.normalizedTime));
        if (side == Side.Right) currentMove.ChargeAttack(animator.IsInTransition(layerIndex));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.tracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);
        currentMove.ResetChargePhase();
    }

}
