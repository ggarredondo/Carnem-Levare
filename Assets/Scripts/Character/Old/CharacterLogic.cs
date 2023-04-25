using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public enum Entity { Player, Enemy }
public enum CharacterState { WALKING, BLOCKING, ATTACKING, HURT, BLOCKED, KO }

public abstract class CharacterLogic : MonoBehaviour
{
    // Character Attributes
    [Header("Tracking values")]
    [SerializeField] private bool debugTracking = true;
    protected Transform target;

    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;
    private Quaternion targetLook;

    [Header("Stats")]

    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 0f;

    [Tooltip("How quickly time disadvantage decreases through consecutive hits (time in ms x number of hits)")]
    [SerializeField] private float comboDecay = 100f;

    [SerializeField] private float attackDamage = 0f;
    [Tooltip("Percentage of stamina damage taken when blocking")] [SerializeField] [Range(0f, 1f)] private float blockingModifier = 0.5f;
    [SerializeField] [InitializationField] [Range(1f, 1.2f)] private float height = 1f;
    [SerializeField] [InitializationField] private float mass = 1f;
    [SerializeField] [InitializationField] private float drag = 0f; // SHOULD BE CALCULATED GIVEN MASS
    [SerializeField] private List<MoveOld> moveset;
    [SerializeField] private List<HitboxOld> hitboxes;

    [Header("Debug")]
    [SerializeField] private bool noDamage = false;
    [SerializeField] private bool noDeath = false;

    // Character Variables
    private Entity entity;
    private CharacterAnimationOld animationHandler;
    private Rigidbody rb;
    protected Vector2 direction, directionTarget;
    protected float directionSpeed;

    [SerializeField] [ReadOnlyField] protected CharacterState state = CharacterState.WALKING;
    private bool blockPressed, isBlocking, canAttack;
    private int moveIndex = 0;
    private bool hurtExceptions;

    [SerializeField] [ReadOnlyField] private float disadvantage;
    [SerializeField] [ReadOnlyField] private int hitCounter;
    public event UnityAction OnAttackPerformed;

    protected virtual void Awake()
    {
        // Initialize Character Attributes
        stamina = maxStamina;
        transform.localScale *= height;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;

        // Initialize Character Variables
        entity = this is PlayerLogic ? Entity.Player : Entity.Enemy;
        animationHandler = GetComponent<CharacterAnimationOld>();
        direction = Vector2.zero;
        directionTarget = Vector2.zero;
    }
    protected virtual void Start()
    {
        animationHandler.OnAttackStart += InitializeAttack;
        animationHandler.OnAttackActive += ActivateHitbox;
        animationHandler.OnAttackRecovery += DeactivateHitbox;
        animationHandler.OnAnimationCancel += ResetState;
    }

    protected virtual void Update()
    {
        hurtExceptions = state == CharacterState.KO || noDamage;
        canAttack = state == CharacterState.WALKING || state == CharacterState.BLOCKING;

        switch (state)
        {
            case CharacterState.WALKING: 
            case CharacterState.BLOCKING:
                state = blockPressed ? CharacterState.BLOCKING : CharacterState.WALKING;
                // Softens movement by establishing the direction as a point that approaches the target direction at *directionSpeed* rate.
                direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);

                hitCounter = 0;
                if (stamina <= 0) state = CharacterState.KO;
                break;

            case CharacterState.ATTACKING:
            case CharacterState.HURT:
            case CharacterState.BLOCKED:
                if (stamina <= 0) state = CharacterState.KO;
                break;
        }
    }
    protected virtual void FixedUpdate()
    {
        // Rotate towards opponent if character is tracking.
        if (target != null && debugTracking && (IsMoving || IsAttacking))
        {
            targetLook = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetLook, trackingRate * Time.fixedDeltaTime);
        }
    }

    #region Actions

    protected void Movement(Vector2 dir) { directionTarget = dir; }
    protected void Block(bool performed) { blockPressed = performed; }
    protected void AttackN(bool performed, int n) {
        if (moveset.Count > n && canAttack) {
            moveIndex = n;
            if (performed) OnAttackPerformed.Invoke();
        }
    }

    #endregion

    #region GameplayFunctions

    private void InitializeAttack()
    {
        state = CharacterState.ATTACKING;
        // Assign move data to hitbox. Must be done this way because hitboxes are reusable.
        hitboxes[(int)moveset[moveIndex].HitboxType].Set(moveset[moveIndex].Power, 
            CalculateAttackDamage(moveset[moveIndex].BaseDamage),
            moveset[moveIndex].Unblockable,
            moveset[moveIndex].HitSound,
            moveset[moveIndex].BlockedSound,
            moveset[moveIndex].AdvantageOnBlock,
            moveset[moveIndex].AdvantageOnHit);
    }
    private void ActivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(true); }
    private void DeactivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(false); }
    private void ResetState() {
        state = blockPressed ? CharacterState.BLOCKING : CharacterState.WALKING;
    }

    /// <summary>
    /// Calculates decreasing time disadvantage after consecutive hits,
    /// so that combos aren't infinite.
    /// </summary>
    /// <param name="disadvantage">Current disadvantage in milliseconds.</param>
    /// <param name="hitNumber">Number of consecutive hits.</param>
    /// <param name="rate">Decreasing rate in milliseconds.</param>
    /// <returns>Current time disadvantage.</returns>
    private float DisadvantageDecay(float disadvantage, float hitNumber, float rate) {
        return disadvantage - hitNumber * rate;
    }

    /// <summary>
    /// Damage character, reducing their stamina and playing a hurt animation.
    /// </summary>
    /// <param name="dmg">Damage taken.</param>
    /// <param name="unblockable">Can the attack be blocked?</param>
    /// <param name="disadvantageOnBlock">How much time (ms) does the character take in blocked animation.</param>
    /// <param name="disadvantageOnHit">How much time (ms) does the character take in hit animation.</param>
    public virtual void Damage(float dmg, bool unblockable, float disadvantageOnBlock, float disadvantageOnHit)
    {
        isBlocking = (state == CharacterState.BLOCKING || state == CharacterState.BLOCKED) && !unblockable;
        state = isBlocking ? CharacterState.BLOCKED : CharacterState.HURT;

        disadvantage = isBlocking ? disadvantageOnBlock : disadvantageOnHit;
        disadvantage = DisadvantageDecay(disadvantage, hitCounter, comboDecay);
        hitCounter += 1;

        stamina -= isBlocking ? Mathf.Round(dmg * blockingModifier) : dmg; // Take less damage if blocking.
        if (stamina <= 0f) stamina = noDeath ? 1f : 0f; // Stamina can't go lower than 0. Can't go lower than 1 if noDeath is activated.
    }

    /// <summary>
    /// Calculate how much damage a character deals to the opponent's stamina when a Move connects.
    /// </summary>
    /// <param name="baseDmg">Move's base damage</param>
    /// <returns>Calculated final damage</returns>
    public float CalculateAttackDamage(float baseDmg) { return baseDmg + attackDamage; }

    #endregion

    #region PublicMethods

    public Entity Entity { get => entity; }
    public CharacterState State { get => state; }

    public int currentIndex { get => moveIndex; }
    public MoveOld currentMove { get => moveset[moveIndex]; }
    public List<MoveOld> Moveset { get => moveset; }
    public List<HitboxOld> Hitboxes { get => hitboxes; }

    public float Stamina { get => stamina; }
    public float MaxStamina { get => maxStamina; }

    public float Disadvantage { get => disadvantage; }

    /// <summary>
    /// Returns character's current intended direction.
    /// </summary>
    public Vector2 DirectionTarget { get => directionTarget; }

    /// <summary>
    /// Returns character's current lerp-smoothed direction.
    /// </summary>
    public Vector2 Direction { get => direction; }

    public bool IsMoving { get => directionTarget.magnitude != 0f && (state == CharacterState.WALKING || state == CharacterState.BLOCKING); }
    public bool IsIdle { get => directionTarget.magnitude == 0f && (state == CharacterState.WALKING || state == CharacterState.BLOCKING); }
    public bool CanAttack { get => canAttack; }
    public bool IsAttacking { get => state == CharacterState.ATTACKING; }
    public bool IsKO { get => state == CharacterState.KO; }
    public bool IsBlockPressed { get => blockPressed; }
    public bool IsBlocking { get => isBlocking; }
    public bool HurtExceptions { get => hurtExceptions; }

    #endregion
}
