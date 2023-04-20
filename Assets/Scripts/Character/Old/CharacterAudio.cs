using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    CharacterLogic logicHandler;
    CharacterAnimation animationHandler;

    private void Awake()
    {
        logicHandler = GetComponent<CharacterLogic>();
        animationHandler = GetComponent<CharacterAnimation>();
    }
    private void Start()
    {
        animationHandler.OnAttackStart += () => AudioManager.Instance.gameSfxSounds.Play(logicHandler.currentMove.WhiffSound, (int)logicHandler.Entity);
    }

    public void PlayHurtSound(string blockedSound, string hitSound)
    {
        AudioManager.Instance.gameSfxSounds.Play(logicHandler.IsBlocking ? blockedSound : hitSound, (int)logicHandler.Entity);
    }
}
