using UnityEngine;

public class EnemyController : Character
{
    [Header("Movement Parameters")]
    [SerializeField] [Range(-1f, 1f)] private float horizontal = 0f;
    [SerializeField] [Range(-1f, 0f)] private float vertical = 0f;

    private void Awake()
    {
        init();
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
    }
}
