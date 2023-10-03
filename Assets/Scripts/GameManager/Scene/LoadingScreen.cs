using LerpUtilities;
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

    private string continueAction;
    private float actualProgress;
    private bool isStopped;

    private void Start()
    {
        GameManager.Input.ControlsChangedEvent += ChangeText;
        ChangeText();
    }

    private void OnEnable()
    {
        SceneLoader.ActivateLoading += Activate;
        SceneLoader.UpdateLoading += UpdateProgess;
    }

    private void OnDisable()
    {
        SceneLoader.ActivateLoading -= Activate;
        SceneLoader.UpdateLoading -= UpdateProgess;
    }

    private void OnDestroy()
    {
        GameManager.Input.ControlsChangedEvent -= ChangeText;
    }

    private async void ProgressBar()
    {
        await Lerp.Value_Math(progressBar.transform.localScale.x, actualProgress, 
                              (p) => progressBar.transform.localScale = new Vector3(p, progressBar.transform.localScale.y, progressBar.transform.localScale.z), 
                              progressBarSpeed,
                              CameraUtilities.Lineal);
    }

    public void StopMask()
    {
        if (!isStopped)
        {
            isStopped = true;
            GameManager.Audio.Play("MaskAlert");
            GameManager.Input.Rumble(0.2f, 1f, 1f);
            maskAnim.speed = 6;
            maskAnim.SetBool("Stop", true);
        }
    }

    public void Activate()
    {
        GameManager.Input.EnablePlayerInput(true);
        GameManager.Input.SwitchActionMap("LoadingScreen");
    }

    public bool UpdateProgess(float progress)
    {
        actualProgress = Mathf.Clamp01(progress / 0.9f);
        percentage.text = (int)(Mathf.Clamp01(progress / 0.9f) * 100) + " %";
        ProgressBar();

        bool result = false;

        if (progress >= 0.9f)
        {
            percentage.text = "Press " + continueAction + " to continue";
            loadingTextAnim.enabled = true;

            if (GameManager.Input.FindAction("Continue").IsPressed())
            {
                GameManager.Audio.Play("ExitLoading");
                result = true;
            }
        }

        return result;
    }

    public void ChangeText()
    {
        continueAction = GameManager.Input.FindAction("Continue").bindings[GameManager.Input.ControlSchemeIndex].path.Split("/")[1];
    }

}
