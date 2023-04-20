using System.Collections;
using UnityEngine;

public class TransitionPlayer : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SceneLoader.startTransition += StartTransition;
        SceneLoader.endTransition += EndTransition;
    }

    private void OnDisable()
    {
        SceneLoader.startTransition -= StartTransition;
        SceneLoader.endTransition -= EndTransition;
    }

    private IEnumerator StartTransition()
    {
        animator.SetBool("isLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    private IEnumerator EndTransition()
    {
        animator.SetBool("endLoading", true);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
