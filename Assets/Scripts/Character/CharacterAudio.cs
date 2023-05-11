
[System.Serializable]
public class CharacterAudio
{
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStateMachine stateMachineLocal = stateMachine;
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => AudioController.Instance.gameSfxSounds.Play(statsValueLocal.MoveList[index].InitSound);
        stateMachine.HurtState.OnEnter += () => AudioController.Instance.gameSfxSounds.Play(stateMachineLocal.HurtState.Hitbox.HitSound);
        stateMachine.KOState.OnEnter += () => AudioController.Instance.gameSfxSounds.Play(stateMachineLocal.KOState.Hitbox.HitSound);
        stateMachine.BlockedState.OnEnter += () => AudioController.Instance.gameSfxSounds.Play(stateMachineLocal.BlockedState.Hitbox.BlockedSound);
    }
}
