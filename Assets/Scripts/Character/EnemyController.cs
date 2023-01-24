using UnityEngine;

public class EnemyController : Character
{
    [Header("Movement Parameters")]
    [Range(-1f, 1f)] public float horizontal = 0f;
    [Range(-1f, 0f)] public float vertical = 0f;


    private void Awake()
    {
        init();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimationParameters();
    }

    //***ANIMATION***

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z)); // Rotate towards player
        anim.applyRootMotion = !anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"); // DELETE when animations are correct
    }
}
