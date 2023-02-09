using UnityEngine;

public abstract class AttackManager : StateMachineBehaviour
{
    protected Character character;
    protected Move currentMove;
    private bool currentMoveFound;
    protected GameObject currentHitbox;
    protected Side side;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentMoveFound = false;
        for (int i = 0; i < character.getLeftMoveset.Count && !currentMoveFound; ++i)
        {
            if (stateInfo.IsName("Left" + i)) {
                currentMove = character.getLeftMoveset[i];
                side = Side.Left;
                currentHitbox = character.leftHitboxes[(int)currentMove.hitboxType];
                currentMoveFound = true; // end loop
            }

            if (stateInfo.IsName("Right" + i)) {
                currentMove = character.getRightMoveset[i];
                side = Side.Right;
                currentHitbox = character.rightHitboxes[(int)currentMove.hitboxType];
                currentMoveFound = true; // end loop
            }
        }

        // Assigns the move's power and damage to the hitbox component so that once it hits the information is passed onto the hurtbox.
        currentHitbox.GetComponent<Hitbox>().power = currentMove.power;
        currentHitbox.GetComponent<Hitbox>().damage = character.CalculateAttackDamage(currentMove.baseDamage);
        // Assign the move's direction by checking if it's straight, and if it's not we assign left o right.
        currentHitbox.GetComponent<Hitbox>().side = currentMove.direction == Direction.Straight ? 0 : (int) side;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.tracking = currentMove.isTracking(side, stateInfo.normalizedTime);
        currentHitbox.GetComponent<Hitbox>().Activate(currentMove.isHitboxActive(side, stateInfo.normalizedTime));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.tracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);
    }
}
