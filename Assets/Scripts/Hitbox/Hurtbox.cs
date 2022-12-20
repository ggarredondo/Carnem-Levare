using UnityEngine;

public enum Target : uint
{
    LeftHead = 0,
    MidHead = 1,
    RightHead = 2,
    LeftBody = 3,
    MidBody = 4,
    RightBody = 5
}

public class Hurtbox : MonoBehaviour
{
    public Animator anim;
    public Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("is_hurt", true);
        anim.SetFloat("hurt_target", (float) target);
        anim.SetFloat("hurt_power", 0);
    }
}
