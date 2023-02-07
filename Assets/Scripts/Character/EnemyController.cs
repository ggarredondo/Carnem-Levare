using UnityEngine;

public class EnemyController : Character
{
    [Header("Movement Parameters")]
    [SerializeField] [Range(-1f, 1f)] private float horizontal = 0f;
    [SerializeField] [Range(-1f, 1f)] private float vertical = 0f;
    [SerializeField] private bool isBlocking = false;

    private void Awake()
    {
        init();
    }

    private void Update()
    {
        SetAnimationParameters();
    }

    private void FixedUpdate()
    {
        fixedUpdating();
    }

    //***ANIMATION***

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        anim.SetBool("block", isBlocking);
        // Ternary operator so that when the enemy isn't moving, the speed parameter doesn't affect the idle animation
        anim.SetFloat("casual_walk_speed", horizontal + vertical == 0f ? 1f : casualWalkingSpeed);
        anim.SetFloat("guard_walk_speed", horizontal + vertical == 0f ? 1f : guardWalkingSpeed);
    }
}
