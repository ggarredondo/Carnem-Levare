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
    public Power powerDebug = Power.Light;
    
    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("hurt");
        anim.SetFloat("hurt_target", (float) target);
        anim.SetFloat("hurt_power", (float) powerDebug);
    }
}
