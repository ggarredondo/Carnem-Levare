using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator anim;

    public Transform target;

    [Header("Stats")]
    public float stamina = 0f;
    public float maxStamina = 0f;
    // Fill with common variables once EnemyController and PlayerController are finished

    public Animator getAnimator { get { return anim; } }
}
