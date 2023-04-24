using System.Threading.Tasks;
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

    private async Task StartTransition()
    {
        animator.SetBool("isLoading", true);
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Task.Delay(System.TimeSpan.FromSeconds(animator.GetCurrentAnimatorStateInfo(0).length));
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }

    private async Task EndTransition()
    {
        animator.SetBool("endLoading", true);
        GameManager.PlayerInput.enabled = false;
        GameManager.UiInput.enabled = false;
        await Task.Delay(System.TimeSpan.FromSeconds(animator.GetCurrentAnimatorStateInfo(0).length));
        GameManager.UiInput.enabled = true;
        GameManager.PlayerInput.enabled = true;
    }
}
