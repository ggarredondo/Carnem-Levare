using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public abstract class Character : MonoBehaviour
{
    // Character Attributes
    [Header("Tracking values")]
    [SerializeField] private bool tracking = true;
    [System.NonSerialized] public bool attackTracking = true; // To deactivate tracking during the commitment phase of an attack.
    protected bool otherTracking = true; // To deactivate tracking during any other action if necessary.
    protected Transform target;

    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;
    private Quaternion targetLook;

    [Header("Stats")]

    [SerializeField] private float stamina;
    [SerializeField] [InitializationField] private float maxStamina = 0f;
    [Tooltip("How fast stamina updates")] [SerializeField] private float staminaSpeed;

    [SerializeField] private float attackDamage = 0f;
    [Tooltip("Attack animation speed")] [InitializationField] [Range(0f, 2f)] public float attackSpeed = 1f;
    [Tooltip("Percentage of stamina damage taken when blocking")] [SerializeField] [Range(0f, 1f)] private float blockingModifier = 0.5f;
    [SerializeField] [InitializationField] [Range(1f, 1.3f)] private float height = 1f;
    [SerializeField] [InitializationField] private float mass = 1f;
    [SerializeField] [InitializationField] private float drag = 0f; // SHOULD BE CALCULATED GIVEN MASS
    [SerializeField] private List<Move> leftMoveset, rightMoveset;

    [Header("Hitbox Lists - Same items as HitboxType enum")]
    [SerializeField] private List<GameObject> leftHitboxes;
    [SerializeField] private List<GameObject> rightHitboxes;

    // Character Variables
    protected Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;
    private Rigidbody rb;

    protected Vector2 direction, directionTarget;
    protected float directionSpeed;

    protected bool isAttacking, isHurt, isBlocking, block_pressed = false;

    [Header("Debug")] // DEBUG
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
        // Bellow are values that must be updated frame by frame to allow certain animations to play out accordingly.
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsTag("Attacking");
        isHurt = anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt");
        isBlocking = (anim.GetCurrentAnimatorStateInfo(0).IsName("Block") || anim.GetCurrentAnimatorStateInfo(0).IsName("Blocked")) && block_pressed;

        // Character can only attack if they're not attacking already or hurt.
        anim.SetBool("can_attack", !isAttacking && !isHurt);
        anim.SetBool("block", block_pressed);
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
        // Rotate towards opponent if character is tracking.
        if (target != null && tracking && attackTracking && otherTracking)
        {
            targetLook = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetLook, trackingRate * Time.fixedDeltaTime);
        }
        else {
            //dir.x = direction.x;
            //dir.z = direction.y;
            //targetLook = Quaternion.LookRotation(dir);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetLook, trackingRate * Time.fixedDeltaTime);
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
            UpdateAnimator("LeftClip" + i, leftMoveset[i].leftAnimation);
            anim.SetFloat("left" + i + "_speed", leftMoveset[i].LeftAnimationSpeed * attackSpeed);

            // Right Moves
            UpdateAnimator("RightClip" + i, rightMoveset[i].rightAnimation);
            anim.SetFloat("right" + i + "_speed", rightMoveset[i].RightAnimationSpeed * attackSpeed);
        }
    }
    #endregion

    #region GameplayFunctions
    /// <summary>
    /// Damage character's stamina.
    /// </summary>
    /// <param name="dmg">Damage taken.</param>
    public void Damage(float dmg) {
        stamina -= isBlocking ? Mathf.Round(dmg * blockingModifier) : dmg;
        if (stamina < 0) stamina = 0;
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

    public List<GameObject> LeftHitboxes { get => leftHitboxes; }
    public List<GameObject> RightHitboxes { get => rightHitboxes; }

    public float Stamina { get => stamina; }
    public float MaxStamina { get => maxStamina; }

    /// <summary>
    /// To state if a move from right moveset is being pressed. Necessary for
    /// charging attacks.
    /// </summary>
    /// <param name="i">Move index in right moveset list</param>
    /// <param name="b">Press value</param>
    public void PressMove(int i, bool b) { rightMoveset[i].pressed = b; }
    #endregion
}
