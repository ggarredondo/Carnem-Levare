using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator anim;

    public Transform target;

    [Header("Stats")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina = 0f;
    // Fill with common variables once EnemyController and PlayerController are finished

    protected void init()
    {
        stamina = maxStamina;
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
