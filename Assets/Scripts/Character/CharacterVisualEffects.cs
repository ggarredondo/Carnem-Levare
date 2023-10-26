using UnityEngine;

[System.Serializable]
public class CharacterVisualEffects
{
    [SerializeField] ParticlesController particlesController;

    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => particlesController.Play(statsValueLocal.MoveList[index].InitParticles.ID,
                                                                                         statsValueLocal.MoveList[index].InitParticles.prefab);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => particlesController.Play(hitbox.HurtHeight.ToString(), hitbox.HurtParticlesPrefab);
        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => particlesController.Play(hitbox.HurtHeight.ToString(), hitbox.BlockedParticlesPrefab);
        stateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => particlesController.Play(hitbox.HurtHeight.ToString(), hitbox.StaggerParticlesPrefab);
        stateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => particlesController.Play(hitbox.HurtHeight.ToString(), hitbox.KOParticlesPrefab);
    }
}
