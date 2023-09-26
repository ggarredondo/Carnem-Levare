using UnityEngine;
public enum HurtHeight : int { Low, High }
public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Character character;
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
        hitbox.SetHeight(hurtHeight);
        stateMachine.OnHurt?.Invoke(hitbox);
        character.ParticlesController.Play(hurtHeight.ToString(), hitbox.Particles);
    }
}
