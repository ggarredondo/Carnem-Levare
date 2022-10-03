using UnityEngine;

public class AnimationEventSounds : MonoBehaviour
{

    private AudioManager sfxManager;

    private void Awake()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
    }

    public void RightFootAnimationEvent()
    {
        sfxManager.Play("Right Foot");
    }

    public void LeftFootAnimationEvent()
    {
        sfxManager.Play("Left Foot");
    }
}
