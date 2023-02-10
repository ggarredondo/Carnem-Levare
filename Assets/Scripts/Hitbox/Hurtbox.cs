using UnityEngine;

public enum Target : uint
{
    Body = 0,
    Head = 1
}

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Hitbox>().hit = true;
        character.Animator.SetTrigger("hurt");
        character.Animator.SetFloat("hurt_side", other.GetComponent<Hitbox>().side);
        character.Animator.SetFloat("hurt_target", (float) target);
        character.Animator.SetFloat("hurt_power", (float) other.GetComponent<Hitbox>().power);
        character.Damage(other.GetComponent<Hitbox>().damage);
    }
}
