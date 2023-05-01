
public class CharacterAudio
{
    private readonly Character character;
    public CharacterAudio(in Character character)
    {
        this.character = character;
        this.character.AttackingState.OnEnter += (int index) => AudioManager.Instance.gameSfxSounds.Play(this.character.Stats.MoveList[index].InitSound);
    }
}
