using UnityEngine;

public abstract class AttackManager : StateMachineBehaviour
{
    protected Character character;
    protected Move currentMove;
    private bool currentMoveFound, active;
    protected GameObject currentHitbox;
    protected Side side;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentMoveFound = false;
        for (int i = 0; i < character.LeftMoveset.Count && !currentMoveFound; ++i)
        {
            if (stateInfo.IsName("Left" + i)) {
                currentMove = character.LeftMoveset[i];
                side = Side.Left;
                currentHitbox = character.LeftHitboxes[(int)currentMove.hitboxType];
                currentMoveFound = true;
            }

            if (stateInfo.IsName("Right" + i)) {
                currentMove = character.RightMoveset[i];
                side = Side.Right;
                currentHitbox = character.RightHitboxes[(int)currentMove.hitboxType];
                currentMoveFound = true;
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
        active = currentMove.isActive(side, stateInfo.normalizedTime);

        if (active) character.transform.position += character.transform.forward * currentMove.getMovement(side) * Time.deltaTime; // Extra movement
        character.attackTracking = !active; // Won't track opponent when move is active
        currentHitbox.GetComponent<Hitbox>().Activate(active); // Hitbox is activated when move is active
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.attackTracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);
    }
}
