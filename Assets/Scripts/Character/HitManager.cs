using UnityEngine;

public class HitManager : StateMachineBehaviour
{
    private enum Hit { Blocked, Hurt }
    [SerializeField] private Hit hit;
    [SerializeField] private Entity entity;
    private Character character;

    private float disadvantage;
    private float timer;
    private const float secToMsec = 1000f;

    private void Awake()
    {
        character = entity == Entity.Player
            ? GameObject.FindGameObjectWithTag("Player").GetComponent<Character>()
            : GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        disadvantage = character.Disadvantage;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime * secToMsec;
        if (timer > disadvantage)
            animator.SetTrigger("cancel");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("cancel");
    }
}
