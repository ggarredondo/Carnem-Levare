using UnityEngine;
public enum Entity { Player, Enemy }

public class AttackManager : StateMachineBehaviour
{
    [SerializeField] private Entity entity;

    private Character character;
    private Move currentMove;
    private bool currentMoveFound, active;
    private GameObject currentHitbox;
    private Side side;

    private void Awake()
    {
        character = entity == Entity.Player 
            ? GameObject.FindGameObjectWithTag("Player").GetComponent<Character>() 
            : GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentMoveFound = false;
        for (int i = 0; i < character.LeftMoveset.Count && !currentMoveFound; ++i)
        {
            if (stateInfo.IsName("Left" + i)) {
                currentMove = character.LeftMoveset[i];
                side = Side.Left;
                currentHitbox = character.LeftHitboxes[(int)currentMove.HitboxType];
                currentMoveFound = true;
            }

            if (stateInfo.IsName("Right" + i)) {
                currentMove = character.RightMoveset[i];
                side = Side.Right;
                currentHitbox = character.RightHitboxes[(int)currentMove.HitboxType];
                currentMoveFound = true;
            }
        }

        // Assigns the move values to the hitbox component so that once it hits the information is passed onto the hurtbox.
        currentHitbox.GetComponent<Hitbox>().power = currentMove.Power;
        currentHitbox.GetComponent<Hitbox>().damage = character.CalculateAttackDamage(currentMove.BaseDamage);
        currentHitbox.GetComponent<Hitbox>().unblockable = currentMove.Unblockable;
        currentHitbox.GetComponent<Hitbox>().hitSound = currentMove.HitSound;

        // Play whiff sound, since character hasn't hit already.
        SoundEvents.Instance.PlaySfx(currentMove.WhiffSound, entity);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = currentMove.IsActive(side, stateInfo.normalizedTime);

        // ACTIVE
        if (active) character.transform.position += character.transform.forward * currentMove.GetMovement(side) * Time.deltaTime; // Extra movement
        character.attackTracking = !active; // Won't track opponent when move is active
        currentHitbox.GetComponent<Hitbox>().Activate(active); // Hitbox is activated

        // RECOVERY
        if (currentMove.HasRecovered(side, stateInfo.normalizedTime)) animator.SetTrigger("cancel"); // Can cancel animation after recovery
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.attackTracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);
        animator.ResetTrigger("cancel");
    }
}
