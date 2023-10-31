using System.Collections;
using UnityEngine;

public class LoadingMask : MonoBehaviour
{
    [Header("Mask")]
    [SerializeField] private Animator maskAnim;
    [SerializeField] private float speed;
    [SerializeField] private TextGenerator textGenerator;

    private bool isStopped, firstTime;

    public void StopMask()
    {
        if (!isStopped)
        {
            isStopped = true;
            GameManager.Audio.Play("MaskAlert");
            GameManager.Input.Rumble(0.2f, 1f, 1f);
            maskAnim.speed = speed;
            maskAnim.SetBool("Stop", true);
        }
    }

    public void ReproduceText()
    {
        if (isStopped && !firstTime) StartCoroutine(TextAfterTime(1));
        if (isStopped && firstTime) textGenerator.NextLine();
    }

    private IEnumerator TextAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        firstTime = true;
        textGenerator.NextLine();
    }
}
