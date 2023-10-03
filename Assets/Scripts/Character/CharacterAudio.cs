
[System.Serializable]
public class CharacterAudio
{
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => GameManager.Audio.Play(statsValueLocal.MoveList[index].InitSound);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.Audio.Play(hitbox.HurtSound);
        stats.OnHyperarmorHurt += (in Hitbox hitbox) => GameManager.Audio.Play(hitbox.HurtSound);
        stateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.Audio.Play(hitbox.KOSound);

        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.Audio.Play(hitbox.BlockedSound);
        stateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.Audio.Play(hitbox.StaggerSound);
    }
}
