using UnityEngine;

public enum Target : uint
{
    LeftHead = 0,
    MidHead = 1,
    RightHead = 2,
    BackHead = 3,

    LeftBody = 4,
    MidBody = 5,
    RightBody = 6,
    BackBody = 7,
}

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Hitbox>().hit) {
            other.GetComponent<Hitbox>().hit = true;
            character.Animator.SetTrigger("hurt");
            character.Animator.SetFloat("hurt_target", (float)target);
            character.Animator.SetFloat("hurt_power", (float)other.GetComponent<Hitbox>().power);
            character.Damage(other.GetComponent<Hitbox>().damage);
        }
    }
}
