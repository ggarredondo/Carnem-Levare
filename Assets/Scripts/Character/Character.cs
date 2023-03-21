using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using System;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected InputReader inputReader;

    // Character Attributes
    [Header("Tracking values")]
    [SerializeField] private bool debugTracking = true;
    [System.NonSerialized] public bool attackTracking = true; // To deactivate tracking during the commitment phase of an attack.
    private bool trackingConditions;
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
    [SerializeField] private List<Move> leftMoveset, rightMoveset;
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
    private bool hurtExceptions;
    private float disadvantage;

    [Header("Debug")] // DEBUG
    [SerializeField] private bool noDamage = false; // DEBUG
    [SerializeField] private bool noDeath = false; // DEBUG
    [SerializeField] private bool updateMoveset = false; // DEBUG
    [Tooltip("Only works on Player")] [SerializeField] private bool modifyTimeScale = false; // DEBUG
    [Tooltip("Only works on Player")] [SerializeField] [Range(0f, 1f)] private float timeScale = 1f; // DEBUG

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

        // Bellow are values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") && !anim.IsInTransition(0);
        isBlocked = anim.GetCurrentAnimatorStateInfo(0).IsName("Blocked");
        isHurt = anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt");
        isBlocking = (anim.GetCurrentAnimatorStateInfo(0).IsName("Block") || isBlocked) && anim.GetBool("block");

        // Character can only attack if they're not attacking already or hurt.
        anim.SetBool("can_attack", !isAttacking && !isHurt && !isBlocked && !isKO);
        anim.SetBool("is_blocking", isBlocking);

        // Softens movement by establishing the direction as a point that approaches the target direction at *directionSpeed* rate.
        direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);

        // DEBUG
        if (updateMoveset) { InitializeMoveset(); updateMoveset = false; } // DEBUG
        if (modifyTimeScale && this is Player) Time.timeScale = timeScale; // DEBUG
    }
    protected virtual void FixedUpdate()
    {
        trackingConditions = debugTracking && attackTracking && !isHurt && !IsIdle && !isKO;
        // Rotate towards opponent if character is tracking.
        if (target != null && trackingConditions)
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
        Assert.AreEqual(leftMoveset.Count, rightMoveset.Count, "Left and Right movesets must have the same number of items");

        for (int i = 0; i < leftMoveset.Count; ++i)
        {
            // Left Moves
            UpdateAnimator("LeftClip" + i, leftMoveset[i].Animation);
            anim.SetFloat("left" + i + "_speed", leftMoveset[i].AnimationSpeed);

            // Right Moves
            UpdateAnimator("RightClip" + i, rightMoveset[i].Animation);
            anim.SetFloat("right" + i + "_speed", rightMoveset[i].AnimationSpeed);
        }
    }

    #endregion

    #region Actions

    protected void Movement(Vector2 dir) { directionTarget = dir; }
    protected void Block(bool performed) { anim.SetBool("block", performed); }

    protected void LeftN(bool performed, int n) { if (leftMoveset.Count > n) anim.SetBool("left" + n, performed); }
    protected void RightN(bool performed, int n) { if (rightMoveset.Count > n) anim.SetBool("right" + n, performed); }

    #endregion

    #region GameplayFunctions

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
    public void Damage(float target, float power, float dmg, bool unblockable, string hitSound, string blockedSound, 
        float disadvantageOnBlock, float disadvantageOnHit)
    {
        // Animation
        anim.SetTrigger("hurt");
        anim.SetFloat("hurt_target", target);
        anim.SetFloat("hurt_power", power);
        anim.SetBool("unblockable", unblockable);
        disadvantage = isBlocking ? disadvantageOnBlock : disadvantageOnHit;

        // Sound
        SoundEvents.Instance.PlaySfx(isBlocking ? blockedSound : hitSound, entity);

        // Stamina
        stamina -= isBlocking && !unblockable ? Mathf.Round(dmg * blockingModifier) : dmg; // Take less damage if blocking.
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

    public Animator Animator { get => anim; }

    public List<Move> LeftMoveset { get => leftMoveset; }
    public List<Move> RightMoveset { get => rightMoveset; }
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
    public float Disadvantage { get => disadvantage; }

    #endregion
}
