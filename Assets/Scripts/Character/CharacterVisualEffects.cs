[System.Serializable]
public class CharacterVisualEffects
{
    public ParticlesController particlesController;

    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => particlesController.Play(statsValueLocal.MoveList[index].InitParticles.ID,
                                                                                         statsValueLocal.MoveList[index].InitParticles.prefab);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => hitbox.SetHurtParticles();
        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => hitbox.SetBlockedParticles();
    }
}
