using UnityEngine;

public class PlayerAttackManager : StateMachineBehaviour
{
    private PlayerController player;
    private Move currentMove;
    private GameObject currentHitbox;
    private Side side;
    private CameraEffects cameraFollow;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cameraFollow = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraEffects>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Checks which of the four possible attacks the player is perfoming, then saves a reference to the corresponding move and its hitbox.
        if (stateInfo.IsName("Left Normal")) { 
            currentMove = player.leftNormalSlot;
            side = Side.Left;
            currentHitbox = player.leftHitboxes[(int) currentMove.hitboxType]; 
        }
        else if (stateInfo.IsName("Right Normal")) { 
            currentMove = player.rightNormalSlot;
            side = Side.Right;
            currentHitbox = player.rightHitboxes[(int) currentMove.hitboxType]; 
        }
        else if (stateInfo.IsName("Left Special")) { 
            currentMove = player.leftSpecialSlot;
            side = Side.Left;
            currentHitbox = player.leftHitboxes[(int) currentMove.hitboxType]; 
        }
        else if (stateInfo.IsName("Right Special")) { 
            currentMove = player.rightSpecialSlot;
            side = Side.Right;
            currentHitbox = player.rightHitboxes[(int) currentMove.hitboxType]; 
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
        currentMove.ChargeAttack(side, animator.IsInTransition(layerIndex), player.attackSpeed);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.tracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentMove.ResetChargePhase();
    }

    
}
