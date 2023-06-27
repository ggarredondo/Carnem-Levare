using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Character character;
    private enum HurtHeight : int { Low, High }
    [SerializeField] private HurtHeight hurtHeight;
    private CharacterStateMachine stateMachine;

    private void Start()
    {
        stateMachine = character.StateMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hitbox = other.GetComponent<Hitbox>();
        hitbox.SetActive(false);
        hitbox.SetHeight((float)hurtHeight);
        stateMachine.OnHurt?.Invoke(hitbox);
    }
}
