using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    public Transform TargetPlayer;

    [Header("Animation Parameters")]
    [Range(0f, 1f)] public float hurtCancelTime = 0.1f;

    [Header("Movement Parameters")]
    [Range(-1f, 1f)] public float horizontal = 0f;
    [Range(-1f, 0f)] public float vertical = 0f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimationParameters();
    }

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        transform.LookAt(new Vector3(TargetPlayer.position.x, transform.position.y, TargetPlayer.position.z)); // Rotate towards player
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= hurtCancelTime) { anim.SetBool("is_hurt", false); }
        anim.applyRootMotion = !anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt");
    }
}
