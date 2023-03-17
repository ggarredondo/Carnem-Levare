using UnityEngine;
public enum Entity { Player, Enemy }

public class AttackManager : StateMachineBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Side side;
    [SerializeField] private int index;

    private Character character;
    private MoveWrapper wrapper;
    private bool active;

    private void Awake()
    {
        character = entity == Entity.Player 
            ? GameObject.FindGameObjectWithTag("Player").GetComponent<Character>() 
            : GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

        // Assign move if there's actually a move to assign
        if (index < character.LeftMoveset.Count) {
            wrapper = side == Side.Left
                ? character.LeftMoveset[index]
                : character.RightMoveset[index];
        };
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Assigns the move values to the hitbox component so that once it hits the information is passed onto the hurtbox.
        // It has to be assigned every time the character attacks because hitboxes are reused between moves.
        wrapper.hitbox.Set(wrapper.move.Power, 
            character.CalculateAttackDamage(wrapper.move.BaseDamage), 
            wrapper.move.Unblockable, 
            wrapper.move.HitSound,
            wrapper.move.BlockedSound);

        // Play whiff sound, since character hasn't hit already.
        SoundEvents.Instance.PlaySfx(wrapper.move.WhiffSound, entity);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = wrapper.move.IsActive(side, stateInfo.normalizedTime);

        // ACTIVE
        if (active) character.transform.position += character.transform.forward * wrapper.move.GetMovement(side) * Time.deltaTime; // Extra movement
        character.attackTracking = !active; // Won't track opponent when move is active
        wrapper.hitbox.Activate(active); // Hitbox is activated

        // RECOVERY
        if (wrapper.move.HasRecovered(side, stateInfo.normalizedTime)) animator.SetTrigger("cancel"); // Can cancel animation after recovery
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.attackTracking = true;
        wrapper.hitbox.hitFlag = false;
        wrapper.hitbox.Activate(false);
        animator.ResetTrigger("cancel");
    }
}
