using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;

    public Transform TargetPlayer;

    [Header("Movement Parameters")]
    [Range(-1f, 1f)] public float horizontal = 0f;
    [Range(-1f, 0f)] public float vertical = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        SetAnimationParameters();
        HurtEnemy();
    }

    /// <summary>
    /// Sets general animation parameters for the animator.
    /// </summary>
    private void SetAnimationParameters()
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        transform.LookAt(new Vector3(TargetPlayer.position.x, transform.position.y, TargetPlayer.position.z)); // Rotate towards player
    }

    #region ------DEBUG------
    [Header("Debug Parameters")]
    public bool hurt = false;
    [Range(0, 5)] public uint target = 0;
    [Range(0, 2)] public uint power = 0;

    private void HurtEnemy()
    {
        anim.SetBool("is_hurt", hurt);
        anim.SetFloat("hurt_target", target);
        anim.SetFloat("hurt_power", power);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            anim.applyRootMotion = false;
        hurt = false;
    }
    #endregion
}
