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
    [SerializeField] private Character character;
    [SerializeField] private Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Hitbox>().hit = true;
        character.getAnimator.SetTrigger("hurt");
        character.getAnimator.SetFloat("hurt_target", (float) target);
        character.getAnimator.SetFloat("hurt_power", (float) other.GetComponent<Hitbox>().power);
        character.Damage(other.GetComponent<Hitbox>().damage);
    }
}
