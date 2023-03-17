using UnityEngine;
public enum Entity { Player, Enemy }

public class AttackManager : StateMachineBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Side side;
    [SerializeField] private int index;

    private Character character;
    private MoveWrapper currentWrapper;
    private Move currentMove;
    private Hitbox currentHitbox;
    private bool active;

    private void AssignMove() {
        if (index < character.LeftMoveset.Count) {
            currentWrapper = side == Side.Left
                ? character.LeftMoveset[index]
                : character.RightMoveset[index];

            currentMove = currentWrapper.move;
            currentHitbox = currentWrapper.hitbox;
        }
    }

    private void Awake()
    {
        character = entity == Entity.Player 
            ? GameObject.FindGameObjectWithTag("Player").GetComponent<Character>() 
            : GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
        AssignMove();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Assigns the move values to the hitbox component so that once it hits the information is passed onto the hurtbox.
        currentHitbox.power = currentMove.Power;
        currentHitbox.damage = character.CalculateAttackDamage(currentMove.BaseDamage);
        currentHitbox.unblockable = currentMove.Unblockable;
        currentHitbox.hitSound = currentMove.HitSound;

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
        currentHitbox.Activate(active); // Hitbox is activated

        // RECOVERY
        if (currentMove.HasRecovered(side, stateInfo.normalizedTime)) animator.SetTrigger("cancel"); // Can cancel animation after recovery
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.attackTracking = true;
        currentHitbox.hit = false;
        currentHitbox.Activate(false);
        animator.ResetTrigger("cancel");
    }
}
