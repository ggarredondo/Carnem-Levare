using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Character : MonoBehaviour
{
    protected Animator anim;
    protected AnimatorOverrideController animOverride;
    protected AnimationClip[] animatorDefaults;

    [SerializeField] protected Transform target;

    [Header("Stats")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 0f;
    [Tooltip("Attack animation speed")] [Range(0,2)] public float attackSpeed = 1f;

    [Header("Hitbox Lists - Same items as HitboxType enum")]
    public List<GameObject> leftHitboxes;
    public List<GameObject> rightHitboxes;

    protected void init()
    {
        stamina = maxStamina;
        anim = GetComponent<Animator>();
        animatorDefaults = anim.runtimeAnimatorController.animationClips;
        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
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

    //***GET FUNCTIONS***

    public Animator getAnimator { get { return anim; } }
}
