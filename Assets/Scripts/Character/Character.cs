using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public enum Entity { Player, Enemy }

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
    [SerializeField] [InitializationField] private float maxStamina = 0f;

    [SerializeField] private float attackDamage = 0f;
    [Tooltip("Percentage of stamina damage taken when blocking")] [SerializeField] [Range(0f, 1f)] private float blockingModifier = 0.5f;
    [SerializeField] [InitializationField] [Range(1f, 1.2f)] private float height = 1f;
    [SerializeField] [InitializationField] private float mass = 1f;
    [SerializeField] [InitializationField] private float drag = 0f; // SHOULD BE CALCULATED GIVEN MASS
    [SerializeField] private List<Move> moveset;
    [SerializeField] private List<Hitbox> hitboxes;

    // Character Variables
    private Entity entity;
    protected Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;
    private Rigidbody rb;
    protected Vector2 direction, directionTarget;
    protected float directionSpeed;

    protected bool isAttacking, isHurt, isKO, isBlocked, isBlocking;
    private float disadvantage;
    private Coroutine hurtCoroutine;
    private int moveIndex = 0;
    private bool hurtExceptions;

    [Header("Debug")]
    [SerializeField] private bool noDamage = false;
    [SerializeField] private bool noDeath = false;
    [SerializeField] private bool updateMoveset = false;

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
        InitializeMoveset();
    }
    protected virtual void Update()
    {
        // Character is KO when stamina is equal or below 0.
        isKO = stamina <= 0;
        anim.SetBool("ko", isKO);
        // Character won't be hurt if any of these conditions are met.
        hurtExceptions = isKO || noDamage;

        // Check current state so that certain behaviours may play accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isBlocked = anim.GetCurrentAnimatorStateInfo(0).IsName("Blocked");
        isHurt = anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt");
        isBlocking = (anim.GetCurrentAnimatorStateInfo(0).IsName("Block") || isBlocked);
        anim.SetBool("is_blocking", isBlocking);
        // Character may not attack when they are blocking an attack, when they are hurt or when they are already attacking.
        anim.SetBool("can_attack", !isAttacking && !isBlocked && !isHurt && !isKO);

        // Softens movement by establishing the direction as a point that approaches the target direction at *directionSpeed* rate.
        direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);

        // --------------- DEBUG --------------------
        if (updateMoveset) { InitializeMoveset(); updateMoveset = false; }
    }
    protected virtual void FixedUpdate()
    {
        // Rotate towards opponent if character is tracking.
        if (target != null && debugTracking && !isHurt && !IsIdle && !isKO)
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
    private void InitializeMoveset()
    {
        for (int i = 0; i < moveset.Count; ++i) {
            UpdateAnimator("AttackClip" + i, moveset[i].Animation);
            anim.SetFloat("attack" + i + "_speed", moveset[i].AnimationSpeed);
            moveset[i].AssignEvents();
        }
    }

    #endregion

    #region Actions

    protected void Movement(Vector2 dir) { directionTarget = dir; }
    protected void Block(bool performed) { anim.SetBool("block", performed); }
    protected void AttackN(bool performed, int n) {
        if (moveset.Count > n) {
            moveIndex = n;
            anim.SetBool("attack" + n, performed);
        }
    }

    #endregion

    #region GameplayFunctions

    public void StartAttack()
    {
        AudioManager.Instance.gameSfxSounds.Play(moveset[moveIndex].WhiffSound, (int)entity); // Play sound.
        // Assign move data to hitbox. Must be done this way because hitboxes are reusable.
        hitboxes[(int)moveset[moveIndex].HitboxType].Set(moveset[moveIndex].Power, 
            CalculateAttackDamage(moveset[moveIndex].BaseDamage),
            moveset[moveIndex].Unblockable,
            moveset[moveIndex].HitSound,
            moveset[moveIndex].BlockedSound,
            moveset[moveIndex].AdvantageOnBlock,
            moveset[moveIndex].AdvantageOnHit);
    }
    public void ActivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(true); }
    public void DeactivateHitbox() { hitboxes[(int)moveset[moveIndex].HitboxType].Activate(false); }
    public void CancelAnimation() { anim.SetTrigger("cancel"); }
    private IEnumerator WaitAndCancelAnimation(float time) { 
        yield return new WaitForSeconds(time);
        anim.SetTrigger("cancel");
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
    /// <param name="disadvantageOnBlock">How many deltaseconds does the character take in blocked animation.</param>
    /// <param name="disadvantageOnHit">How many deltaseconds does the character take in hit animation.</param>
    public virtual void Damage(float target, float power, float dmg, bool unblockable, string hitSound, string blockedSound, 
        float disadvantageOnBlock, float disadvantageOnHit)
    {
        // Animation
        anim.SetTrigger("hurt");
        anim.SetFloat("hurt_target", target);
        anim.SetFloat("hurt_power", power);
        anim.SetBool("unblockable", unblockable);
        disadvantage = isBlocking && !unblockable ? disadvantageOnBlock : disadvantageOnHit;

        // Sound
        AudioManager.Instance.gameSfxSounds.Play(isBlocking && !unblockable ? blockedSound : hitSound, (int) entity);

        // Stamina
        stamina -= isBlocking && !unblockable ? Mathf.Round(dmg * blockingModifier) : dmg; // Take less damage if blocking.
        if (stamina <= 0f) stamina = noDeath ? 1f : 0f; // Stamina can't go lower than 0. Can't go lower than 1 if noDeath is activated.

        if (hurtCoroutine != null) StopCoroutine(hurtCoroutine);
        hurtCoroutine = StartCoroutine(WaitAndCancelAnimation(disadvantage / 1000f));
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

    public bool IsMoving { get => directionTarget.magnitude != 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); }
    public bool IsIdle { get => directionTarget.magnitude == 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"); }
    public bool IsAttacking { get => isAttacking; }
    public bool IsBlocking { get => isBlocking; }
    public bool IsKO { get => isKO; }
    public bool HurtExceptions { get => hurtExceptions; }

    #endregion
}
