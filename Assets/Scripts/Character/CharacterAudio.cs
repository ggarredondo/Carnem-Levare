
[System.Serializable]
public class CharacterAudio
{
    public void Initialize() {}
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStateMachine stateMachineLocal = stateMachine;
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => AudioManager.Instance.gameSfxSounds.Play(statsValueLocal.MoveList[index].InitSound);
        stateMachine.HurtState.OnEnter += () => AudioManager.Instance.gameSfxSounds.Play(stateMachineLocal.HurtState.Hitbox.HitSound);
        stateMachine.BlockedState.OnEnter += () => AudioManager.Instance.gameSfxSounds.Play(stateMachineLocal.BlockedState.Hitbox.BlockedSound);
    }
}
