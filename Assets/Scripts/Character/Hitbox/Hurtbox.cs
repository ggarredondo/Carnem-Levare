using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private enum BodyTarget : int
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
    [SerializeField] private BodyTarget target;
    [SerializeField] private Character character;
    private Controller controller;

    private void Start()
    {
        controller = character.Controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hitbox = other.GetComponent<Hitbox>();
        hitbox.SetActive(false);
        hitbox.SetUnblockable(hitbox.Unblockable || target == BodyTarget.BackHead || target == BodyTarget.BackBody);
        hitbox.SetAnimationBodyTarget((float)target);
        controller.OnHurt?.Invoke(hitbox);
    }
}
