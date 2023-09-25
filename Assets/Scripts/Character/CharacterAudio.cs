
[System.Serializable]
public class CharacterAudio
{
    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        CharacterStats statsValueLocal = stats;
        stateMachine.MoveState.OnEnterInteger += (int index) => GameManager.AudioController.Play(statsValueLocal.MoveList[index].InitSound);

        stateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.AudioController.Play(hitbox.HurtSound);
        stats.OnHyperarmorHurt += (in Hitbox hitbox) => GameManager.AudioController.Play(hitbox.HurtSound);
        stateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.AudioController.Play(hitbox.KOSound);

        stateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.AudioController.Play(hitbox.BlockedSound);
        stateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => GameManager.AudioController.Play(hitbox.StaggerSound);
    }
}
