using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator anim;
    // Fill with common variables once EnemyController and PlayerController are finished

    public Animator getAnimator { get { return anim; } }
}
