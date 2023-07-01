
[System.Serializable]
public class CharacterAudio
{
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => AudioController.Instance.gameSfxSounds.Play(statsValueLocal.MoveList[index].InitSound);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.HurtSound);
        stats.OnHyperarmorHurt += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.HurtSound);
        stateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.KOSound);

        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.BlockedSound);
        stateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.StaggerSound);
    }
}
