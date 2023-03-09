using UnityEngine;

public enum Target : uint
{
    LeftHead = 0,
    MidHead = 1,
    RightHead = 2,

    LeftBody = 3,
    MidBody = 4,
    RightBody = 5,

    BackHead = 6,
    BackBody = 7
}

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Target target;
    private bool unblock;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Hitbox>().hit) {
            other.GetComponent<Hitbox>().hit = true;
            character.Animator.SetTrigger("hurt");
            character.Animator.SetFloat("hurt_target", (float)target);
            character.Animator.SetFloat("hurt_power", (float)other.GetComponent<Hitbox>().power);

            unblock = target == Target.BackHead || target == Target.BackBody || other.GetComponent<Hitbox>().unblockable;
            character.Animator.SetBool("unblockable", unblock);
            character.Damage(other.GetComponent<Hitbox>().damage, unblock);
        }
    }
}
