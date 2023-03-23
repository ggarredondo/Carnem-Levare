using UnityEngine;
public enum Entity { Player, Enemy }
public enum Side { Left, Right }

public class AttackManager : StateMachineBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Side side;
    [SerializeField] private int index;

    private Character character;
    private Move move;
    private Hitbox hitbox;
    private bool active;

    private void Awake()
    {
        character = entity == Entity.Player 
            ? GameObject.FindGameObjectWithTag("Player").GetComponent<Character>() 
            : GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

        // Assign move if there's actually a move to assign
        if (index < character.LeftMoveset.Count) {
            move = side == Side.Left
                ? character.LeftMoveset[index]
                : character.RightMoveset[index];

            hitbox = character.Hitboxes.Find(x => x.name == move.HitboxName);
        };
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Assigns the move values to the hitbox component so that once it hits the information is passed onto the hurtbox.
        // It has to be assigned every time the character attacks because hitboxes are reused between moves.
        hitbox.Set(move.Power, 
            character.CalculateAttackDamage(move.BaseDamage), 
            move.Unblockable, 
            move.HitSound,
            move.BlockedSound,
            move.AdvantageOnBlock,
            move.AdvantageOnHit);

        // Play whiff sound, since character hasn't hit already.
        AudioManager.Instance.gameSfxSounds.Play(move.WhiffSound, (int) entity);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = move.IsActive(stateInfo.normalizedTime);

        // ACTIVE
        if (active) character.transform.position += character.transform.forward * move.ExtraMovement * Time.deltaTime; // Extra movement
        character.attackTracking = !active; // Won't track opponent when move is active
        hitbox.Activate(active); // Hitbox is activated

        // RECOVERY
        if (move.HasRecovered(stateInfo.normalizedTime)) animator.SetTrigger("cancel"); // Can cancel animation after recovery
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.attackTracking = true;
        hitbox.hitFlag = false;
        hitbox.Activate(false);
        animator.ResetTrigger("cancel");
    }
}
