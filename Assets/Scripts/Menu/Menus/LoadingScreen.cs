using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading bar")]
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Image progressBar;
    [SerializeField] private float progressBarSpeed;
    [SerializeField] private Animator loadingTextAnim;

    [Header("Mask")]
    [SerializeField] private Animator maskAnim;

    [Header("Transition")]
    [SerializeField] private Animator transAnim;

    private string continueAction;
    private float actualProgress;
    private bool isStopped;

    private void OnEnable(){ ControlSaver.StaticEvent += ChangeText; }

    private void OnDisable(){ ControlSaver.StaticEvent -= ChangeText;}

    private void Update()
    {
        float xScale = Mathf.Lerp(progressBar.transform.localScale.x, actualProgress, progressBarSpeed * Time.deltaTime);
        progressBar.transform.localScale = new Vector3(xScale, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        if (SceneManagement.Instance.PlayerInput.actions.FindAction("Stop").IsPressed() && !isStopped)
        {
            isStopped = true;
            AudioManager.Instance.uiSfxSounds.Play("MaskAlert");
            maskAnim.speed = 6;
            maskAnim.SetBool("Stop", true);
        }
    }

    #region Public

    /// <summary>
    /// Change to the loading screen Action map
    /// </summary>
    public void Activate()
    {
        SceneManagement.Instance.PlayerInput.SwitchCurrentActionMap("LoadingScreen");
    }

    /// <summary>
    /// Update the UI to the current loading progress
    /// </summary>
    /// <param name="progress">The Async operation progress</param>
    /// <returns></returns>
    public bool UpdateProgess(float progress)
    {
        actualProgress = Mathf.Clamp01(progress / 0.9f);
        percentage.text = (int)(Mathf.Clamp01(progress / 0.9f) * 100) + " %";

        bool result = false;

        if (progress >= 0.9f)
        {
            percentage.text = "Press " + continueAction + " to continue";
            loadingTextAnim.enabled = true;

            if (SceneManagement.Instance.PlayerInput.actions.FindAction("Continue").IsPressed())
            {
                AudioManager.Instance.uiSfxSounds.Play("ExitLoading");
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Function executes when OnControlsChanged triggers
    /// </summary>
    public void ChangeText()
    {
        continueAction = SceneManagement.Instance.PlayerInput.actions.FindActionMap("LoadingScreen").FindAction("Continue").bindings[ControlSaver.controlSchemeIndex].path.Split("/")[1];
    }

    #endregion

}
