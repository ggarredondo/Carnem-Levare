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
    [SerializeField] private Target target;
    [SerializeField] private GameObject character;
    private CharacterLogic logicHandler;
    private CharacterAnimation animationHandler;
    private CharacterAudio audioHandler;
    private Hitbox hitbox;

    private void Awake()
    {
        logicHandler = character.GetComponent<CharacterLogic>();
        animationHandler = character.GetComponent<CharacterAnimation>();
        audioHandler = character.GetComponent<CharacterAudio>();
    }

    private void OnTriggerEnter(Collider other)
    {
        hitbox = other.GetComponent<Hitbox>();
        if (!logicHandler.HurtExceptions) {
            // Establish that hitbox has already hit so that it's disabled and it doesn't hit twice.
            hitbox.Activate(false);

            logicHandler.Damage(hitbox.Damage,
                // Can't block attack if it's unblockable or if it hits the back.
                hitbox.Unblockable || target == Target.BackHead || target == Target.BackBody,
                hitbox.AdvantageOnBlock, hitbox.AdvantageOnHit);

            animationHandler.TriggerHurtAnimation((float) target, (float) hitbox.Power);
            audioHandler.PlayHurtSound(hitbox.BlockedSound, hitbox.HitSound);
        }
    }
}
