
[System.Serializable]
public class CharacterAudio
{
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStateMachine stateMachineLocal = stateMachine;
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => AudioController.Instance.gameSfxSounds.Play(statsValueLocal.MoveList[index].InitSound);
        stats.OnHurtDamage += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.HitSound);
        stats.OnBlockedDamage += (in Hitbox hitbox) => AudioController.Instance.gameSfxSounds.Play(hitbox.BlockedSound);
        stateMachine.StaggerState.OnEnter += () => AudioController.Instance.gameSfxSounds.Play(stateMachineLocal.StaggerState.Hitbox.StaggerSound);
    }
}
