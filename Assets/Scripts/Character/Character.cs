using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Character : MonoBehaviour
{
    protected Animator anim;
    protected AnimatorOverrideController animOverride;
    protected AnimationClip[] animatorDefaults;

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
    [SerializeField] [Range(0f, 2f)] protected float casualWalkingSpeed = 1f;
    [SerializeField] [Range(0f, 2f)] protected float guardWalkingSpeed = 1f;
    [SerializeField] [Range(1f, 1.3f)] private float height = 1f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 0f; // SHOULD BE CALCULATED GIVEN MASS
    private Rigidbody rb;

    [Header("Hitbox Lists - Same items as HitboxType enum")]
    public List<GameObject> leftHitboxes;
    public List<GameObject> rightHitboxes;

    protected void init()
    {
        stamina = maxStamina;
        anim = GetComponent<Animator>();
        animatorDefaults = anim.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
        transform.localScale *= height;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;
    }

    protected void fixedUpdating()
    {
        targetLook = Quaternion.LookRotation(target.position - transform.position);
        if (tracking)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetLook, trackingRate * Time.deltaTime); // Rotate towards opponent.
    }

    //***ANIMATION***

    /// <summary>
    /// Updates specific animation from animator in real time.
    /// </summary>
    /// <param name="og_clip">Name of the animation clip to be updated</param>
    /// <param name="new_clip">New animation clip</param>
    protected void UpdateAnimator(string og_clip, AnimationClip new_clip)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(animatorDefaults.Where(clip => clip.name == og_clip).SingleOrDefault(), new_clip));
        animOverride.ApplyOverrides(overrides);
        anim.runtimeAnimatorController = animOverride;
    }

    //***GAMEPLAY***

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

    //***GET FUNCTIONS***

    public Animator getAnimator { get { return anim; } }
}
