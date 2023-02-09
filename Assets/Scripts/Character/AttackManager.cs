using UnityEngine;

public enum CharacterType { Player, Enemy }
public enum ChargePhase { waiting, performing, canceled }

public class AttackManager : StateMachineBehaviour
{
    [SerializeField] private CharacterType characterType;

    private Character character;
    private Move currentMove;
    private bool currentMoveFound;
    private GameObject currentHitbox;
    private Side side;

    // Player only parameters
    private ChargePhase chargePhase;
    private float deltaTimer;
    private CameraEffects cameraEffect;

    private void Awake()
    {
        if (characterType == CharacterType.Player) {
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            cameraEffect = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraEffects>();
        }
        else
            character = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Player>();
    }

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

        if (characterType == CharacterType.Player) {
            // Assign charge attack timings to camera.
            cameraEffect.SetChargeValues(chargePhase, deltaTimer, currentMove.getChargeLimit, currentMove.getChargeLimitDivisor);
            cameraEffect.Initialized();
        }
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
                if (currentMove.pressed && currentMove.getChargeable)
                {
                    deltaTimer = 0f;
                    chargePhase = ChargePhase.performing;
                }
                break;

            case ChargePhase.performing:
                if (currentMove.pressed && !animator.IsInTransition(layerIndex))
                {
                    animator.speed = Mathf.Lerp(animator.speed, 0f, currentMove.getChargeDecay * Time.deltaTime);
                    deltaTimer += Time.deltaTime;
                }

                if (!currentMove.pressed || deltaTimer >= currentMove.getChargeLimit)
                {
                    animator.speed = 1f;
                    chargePhase = ChargePhase.canceled;
                }
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.tracking = currentMove.isTracking(side, stateInfo.normalizedTime);
        currentHitbox.GetComponent<Hitbox>().Activate(currentMove.isHitboxActive(side, stateInfo.normalizedTime));

        if (characterType == CharacterType.Player) {
            if (side == Side.Right) ChargeAttack(animator, layerIndex);
            cameraEffect.SetChargeValues(chargePhase, deltaTimer); // Camera checks Charge timings every update.
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.tracking = true;
        currentHitbox.GetComponent<Hitbox>().hit = false;
        currentHitbox.GetComponent<Hitbox>().Activate(false);

        if (characterType == CharacterType.Player) chargePhase = ChargePhase.waiting;
    }

}
