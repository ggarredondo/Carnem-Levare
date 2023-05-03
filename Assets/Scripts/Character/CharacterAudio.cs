
public class CharacterAudio
{
    private readonly Character character;
    public CharacterAudio(in Character character)
    {
        this.character = character;
        this.character.StateMachine.MoveState.OnEnter += (int index) => AudioManager.Instance.gameSfxSounds.Play(this.character.Stats.MoveList[index].InitSound);
        this.character.StateMachine.HurtState.OnEnter += () => AudioManager.Instance.gameSfxSounds.Play(this.character.StateMachine.HurtState.Hitbox.HitSound);
        this.character.StateMachine.BlockedState.OnEnter += () => AudioManager.Instance.gameSfxSounds.Play(this.character.StateMachine.BlockedState.Hitbox.BlockedSound);
    }
}
