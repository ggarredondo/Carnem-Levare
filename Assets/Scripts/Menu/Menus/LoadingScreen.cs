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

    private void OnEnable()
    {
        SceneLoader.activateLoading += Activate;
        SceneLoader.updateLoading += UpdateProgess;
    }

    private void OnDisable()
    {
        SceneLoader.activateLoading -= Activate;
        SceneLoader.updateLoading -= UpdateProgess;
    }

    private void Update()
    {
        ChangeText();
        float xScale = Mathf.Lerp(progressBar.transform.localScale.x, actualProgress, progressBarSpeed * Time.deltaTime);
        progressBar.transform.localScale = new Vector3(xScale, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        if (GameManager.PlayerInput.actions.FindAction("Stop").IsPressed() && !isStopped)
        {
            isStopped = true;
            AudioManager.Instance.uiSfxSounds.Play("MaskAlert");
            GameManager.ControllerRumble.Rumble(0.2f, 1f, 1f);
            maskAnim.speed = 6;
            maskAnim.SetBool("Stop", true);
        }
    }

    public void Activate()
    {
        GameManager.PlayerInput.enabled = true;
        GameManager.PlayerInput.SwitchCurrentActionMap("LoadingScreen");
    }

    public bool UpdateProgess(float progress)
    {
        actualProgress = Mathf.Clamp01(progress / 0.9f);
        percentage.text = (int)(Mathf.Clamp01(progress / 0.9f) * 100) + " %";

        bool result = false;

        if (progress >= 0.9f)
        {
            ChangeText();
            percentage.text = "Press " + continueAction + " to continue";
            loadingTextAnim.enabled = true;

            if (GameManager.PlayerInput.actions.FindAction("Continue").IsPressed())
            {
                AudioManager.Instance.uiSfxSounds.Play("ExitLoading");
                result = true;
            }
        }

        return result;
    }

    public void ChangeText()
    {
        continueAction = GameManager.PlayerInput.actions.FindAction("Continue").bindings[GameManager.InputDetection.controlSchemeIndex].path.Split("/")[1];
    }

}
