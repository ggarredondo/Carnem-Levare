using UnityEngine;

public class SoundTrigger : StateMachineBehaviour
{
    private AudioManager sfxManager;

    private void Awake()
    {
        sfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioManager>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Left Jab"))
            sfxManager.Play("Left Jab");

        if (stateInfo.IsName("Right Jab"))
            sfxManager.Play("Right Jab");

        if (stateInfo.IsName("Left Special"))
            sfxManager.Play("Left Special");

        if (stateInfo.IsName("Right Special"))
            sfxManager.Play("Right Special");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Left Jab"))
            sfxManager.Stop("Left Jab");

        if (stateInfo.IsName("Right Jab"))
            sfxManager.Stop("Right Jab");

        if (stateInfo.IsName("Left Special"))
            sfxManager.Stop("Left Special");

        if (stateInfo.IsName("Right Special"))
            sfxManager.Stop("Right Special");
    }
}
