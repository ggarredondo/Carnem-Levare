using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public enum Entity { Player, Enemy }
public enum CharacterState { walking, blocking, attacking, hurt, blocked, ko }

public abstract class Character : MonoBehaviour
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
    [SerializeField] private List<Move> moveset;
    [SerializeField] private List<Hitbox> hitboxes;

    [Header("Debug")]
    [SerializeField] private bool noDamage = false;
    [SerializeField] private bool noDeath = false;
    [SerializeField] private bool updateMoveAnimations = false;
    [SerializeField] private bool updateTimeData = false;

    // Character Variables
    private Entity entity;
    protected Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;
    private Rigidbody rb;
    protected Vector2 direction, directionTarget;
    protected float directionSpeed;

    [SerializeField] [ReadOnlyField] protected CharacterState state = CharacterState.walking;
    protected bool block_pressed;
    private bool canAttack, isBlocking;
    [SerializeField] [ReadOnlyField] private float disadvantage;
    [SerializeField] [ReadOnlyField] private int hitCounter;
    private Coroutine hurtCoroutine;
    private int moveIndex = 0;
    private bool hurtExceptions;

    protected virtual void Awake()
    {
        // Initialize Character Attributes
        stamina = maxStamina;
        transform.localScale *= height;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;

        // Initialize Character Variables
        entity = this is Player ? Entity.Player : Entity.Enemy;
        anim = GetComponent<Animator>();
        animatorDefaults = anim.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
        direction = Vector2.zero;
        directionTarget = Vector2.zero;
    }
    protected virtual void Start()
    {
        UpdateMovesetAnimations();
    }

    protected virtual void Update()
    {
        hurtExceptions = state == CharacterState.ko || noDamage;
        canAttack = state == CharacterState.walking || state == CharacterState.blocking;
        anim.SetBool("can_attack", canAttack);
        isBlocking = state == CharacterState.blocking || state == CharacterState.blocked;
        anim.SetBool("is_blocking", isBlocking);

        switch (state)
        {
            case CharacterState.walking: case CharacterState.blocking:
                state = block_pressed ? CharacterState.blocking : CharacterState.walking;

                // Softens movement by establishing the direction as a point that approaches the target direction at *directionSpeed* rate.
                direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);
                anim.SetFloat("horizontal", direction.x);
                anim.SetFloat("vertical", direction.y);

                hitCounter = 0;
                if (stamina <= 0) EnterKOState();
                break;

            case CharacterState.hurt:
                if (stamina <= 0) EnterKOState();
                break;

            case CharacterState.blocked:
                if (stamina <= 0) EnterKOState();
                break;

            case CharacterState.ko:
                if (stamina <= 0) EnterKOState();
                break;
        }

        // --------------- DEBUG --------------------
        if (updateMoveAnimations) { UpdateMovesetAnimations(); updateMoveAnimations = false; }
        if (updateTimeData) { foreach (Move m in moveset) { m.AssignEvents(); } updateTimeData = false; }
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

    #region Animation

    /// <summary>
    /// Updates specific animation from animator in real time.
    /// </summary>
    /// <param name="og_clip">Name of the animation clip to be updated</param>
    /// <param name="new_clip">New animation clip</param>
    private void UpdateAnimator(string og_clip, AnimationClip new_clip)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(animatorDefaults.Where(clip => clip.name == og_clip).SingleOrDefault(), new_clip));
        animOverride.ApplyOverrides(overrides);
        anim.runtimeAnimatorController = animOverride;
    }

    /// <summary>
    /// Assigns moves' animations and speed to animator.
    /// </summary>
    private void UpdateMovesetAnimations()
    {
        for (int i = 0; i < moveset.Count; ++i) {
            UpdateAnimator("AttackClip" + i, moveset[i].Animation);
            anim.SetFloat("attack" + i + "_speed", moveset[i].AnimationSpeed);
        }
    }

    #endregion

    #region Actions

    protected void Movement(Vector2 dir) { directionTarget = dir; }
    protected void Block(bool performed) { block_pressed = performed; }
    protected void AttackN(bool performed, int n) {
        if (moveset.Count > n && canAttack) {
            moveIndex = n;
            if (performed) anim.SetTrigger("attack" + n);
        }
    }

    #endregion

    #region GameplayFunctions

    public void StartAttack()
    {
        anim.ResetTrigger("cancel");
        state = CharacterState.attacking;
        AudioManager.Instance.gameSfxSounds.Play(moveset[moveIndex].WhiffSound, (int)entity); // Play sound.
        // Assign move data to hitbox. Must be done this way because hitboxes are reusable.
        hitboxes[(int)moveset[moveIndex].HitboxType].Set(moveset[moveIndex].Power, 
            CalculateAttackDamage(moveset[moveIndex].BaseDamage),
            moveset[moveIndex].Unblockable,
            moveset[moveIndex].HitSound,
            moveset[moveIndex].BlockedSound,
            moveset[moveIndex].AdvantageOnBlock,
            moveset[moveIndex].AdvantageOnHit);
        GetComponent<Timer>().StartTimer();
    }
    public void ActivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(true); }
    public void DeactivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(false); }
    public void CancelAnimation() {
        state = block_pressed ? CharacterState.blocking : CharacterState.walking;
        anim.SetTrigger("cancel");
        GetComponent<Timer>().StopTimer();
    }
    private IEnumerator WaitAndCancelAnimation(float ms) { 
        yield return new WaitForSeconds(ms / 1000f);
        CancelAnimation();
    }
    private void EnterKOState()
    {
        anim.SetBool("ko", true);
        state = CharacterState.ko;
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
    /// <param name="target">Where were they damaged?</param>
    /// <param name="power">Attack's power.</param>
    /// <param name="dmg">Damage taken.</param>
    /// <param name="unblockable">Can the attack be blocked?</param>
    /// <param name="hitSound">Sound if the attack hits character directly.</param>
    /// <param name="blockedSound">Sound if the attack hits character while blocking.</param>
    /// <param name="disadvantageOnBlock">How much time (ms) does the character take in blocked animation.</param>
    /// <param name="disadvantageOnHit">How much time (ms) does the character take in hit animation.</param>
    public virtual void Damage(float target, float power, float dmg, bool unblockable, string hitSound, string blockedSound, 
        float disadvantageOnBlock, float disadvantageOnHit)
    {
        state = isBlocking ? CharacterState.blocked : CharacterState.hurt;

        // Animation
        anim.SetTrigger("hurt");
        anim.SetFloat("hurt_target", target);
        anim.SetFloat("hurt_power", power);
        anim.SetBool("unblockable", unblockable);

        disadvantage = isBlocking && !unblockable ? disadvantageOnBlock : disadvantageOnHit;
        disadvantage = DisadvantageDecay(disadvantage, hitCounter, comboDecay);
        hitCounter += 1;

        // Sound
        AudioManager.Instance.gameSfxSounds.Play(isBlocking && !unblockable ? blockedSound : hitSound, (int) entity);

        // Stamina
        stamina -= isBlocking && !unblockable ? Mathf.Round(dmg * blockingModifier) : dmg; // Take less damage if blocking.
        if (stamina <= 0f) stamina = noDeath ? 1f : 0f; // Stamina can't go lower than 0. Can't go lower than 1 if noDeath is activated.

        if (hurtCoroutine != null) StopCoroutine(hurtCoroutine);
        GetComponent<Timer>().StartTimer();
        hurtCoroutine = StartCoroutine(WaitAndCancelAnimation(disadvantage)); 
    }

    /// <summary>
    /// Calculate how much damage a character deals to the opponent's stamina when a Move connects.
    /// </summary>
    /// <param name="baseDmg">Move's base damage</param>
    /// <returns>Calculated final damage</returns>
    public float CalculateAttackDamage(float baseDmg) { return baseDmg + attackDamage; }

    #endregion

    #region PublicMethods

    public Animator Animator { get => anim; }

    public List<Move> Moveset { get => moveset; }
    public List<Hitbox> Hitboxes { get => hitboxes; }

    public float Stamina { get => stamina; }
    public float MaxStamina { get => maxStamina; }

    /// <summary>
    /// Returns character's current intended direction.
    /// </summary>
    public Vector2 DirectionTarget { get => directionTarget; }

    /// <summary>
    /// Returns character's current lerp-smoothed direction.
    /// </summary>
    public Vector2 Direction { get => direction; }

    public bool IsMoving { get => directionTarget.magnitude != 0f && state == CharacterState.walking; }
    public bool IsIdle { get => directionTarget.magnitude == 0f && state == CharacterState.walking; }
    public bool IsAttacking { get => state == CharacterState.attacking; }
    public bool IsBlocking { get => isBlocking; }
    public bool IsKO { get => state == CharacterState.ko; }
    public bool HurtExceptions { get => hurtExceptions; }

    #endregion
}
