using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public abstract class Character : MonoBehaviour
{
    // Character Attributes
    [Header("Tracking values")]
    [SerializeField] protected Transform target;
    [System.NonSerialized] public bool tracking = true;

    [Tooltip("How quickly character rotates towards their opponent")]
    [SerializeField] private float trackingRate = 1f;
    private Quaternion targetLook;

    [Header("Stats")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 0f;
    [SerializeField] private float attackDamage = 0f;
    [Tooltip("Attack animation speed")] [Range(0f, 2f)] public float attackSpeed = 1f;
    [SerializeField] [Range(0f, 2f)] private float casualWalkingSpeed = 1f;
    [SerializeField] [Range(0f, 2f)] private float blockWalkingSpeed = 1f;
    [SerializeField] [Range(0f, 2f)] private float skipSpeed = 1f;
    [SerializeField] [Range(1f, 1.3f)] private float height = 1f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 0f; // SHOULD BE CALCULATED GIVEN MASS
    [SerializeField] private List<Move> leftMoveset, rightMoveset;
    private Rigidbody rb;

    [Header("Hitbox Lists - Same items as HitboxType enum")]
    [SerializeField] private List<GameObject> leftHitboxes;
    [SerializeField] private List<GameObject> rightHitboxes;

    // Character Variables
    protected Animator anim;
    private AnimatorOverrideController animOverride;
    private AnimationClip[] animatorDefaults;

    protected Vector2 direction, directionTarget;
    protected float directionSpeed;

    protected bool isBlocking = false;

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
        anim.SetFloat("skip_speed", skipSpeed);

    }

    protected virtual void Update()
    {
        anim.SetBool("block", isBlocking);

        // Ternary operator so that when the character isn't moving, the speed parameter doesn't affect the idle animation
        anim.SetFloat("casual_walk_speed", directionTarget.magnitude == 0f ? 1f : casualWalkingSpeed);
        anim.SetFloat("block_walk_speed", directionTarget.magnitude == 0f ? 1f : blockWalkingSpeed);

        // Softens movement by establishing the direction as a point that approaches the target direction at *directionSpeed* rate.
        direction = Vector2.Lerp(direction, directionTarget, directionSpeed * Time.deltaTime);
        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);

        if (updateMoveset) { InitializeMoveset(); updateMoveset = false; } // DEBUG
        if (modifyTimeScale && this is Player) Time.timeScale = timeScale; // DEBUG
    }

    protected virtual void FixedUpdate()
    {
        targetLook = Quaternion.LookRotation(target.position - transform.position);
        if (tracking)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetLook, trackingRate * Time.deltaTime); // Rotate towards opponent.
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
    public void Damage(float dmg)
    {
        stamina -= Mathf.Abs(dmg);
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
    public Animator Animator { get { return anim; } }
    public List<Move> LeftMoveset { get { return leftMoveset; } }
    public List<Move> RightMoveset { get { return rightMoveset; } }
    public List<GameObject> LeftHitboxes { get { return leftHitboxes; } }
    public List<GameObject> RightHitboxes { get { return rightHitboxes; } }

    /// <summary>
    /// To state if a move from right moveset is being pressed. Necessary for
    /// charging attacks.
    /// </summary>
    /// <param name="i">Move index in right moveset list</param>
    /// <param name="b">Press value</param>
    public void PressMove(int i, bool b) { rightMoveset[i].pressed = b; }
    #endregion
}
