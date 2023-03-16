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
    private Hitbox hitbox;
    
    private void OnTriggerEnter(Collider other)
    {
        hitbox = other.GetComponent<Hitbox>();
        if (!hitbox.hit && !character.HurtExceptions) {
            // Establish that hitbox has already hit so that it's disabled and it doesn't hit twice.
            hitbox.hit = true;

            character.Damage((float) target, (float) hitbox.power, hitbox.damage,
                // Can't block attack if it's unblockable or if it hits the back.
                hitbox.unblockable || target == Target.BackHead || target == Target.BackBody);

            // Play hit sound now that we know for sure it hit.
            SoundEvents.Instance.PlaySfx(other.GetComponent<Hitbox>().hitSound);
        }
    }
}
