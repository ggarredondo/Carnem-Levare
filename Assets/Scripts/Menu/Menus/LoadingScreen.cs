using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    [Header("Loading bar")]
    [SerializeField] private TMP_Text percentage;
    [SerializeField] private Image progressBar;
    [SerializeField] private float progressBarSpeed;
    [SerializeField] private Animator loadingTextAnim;

    [Header("Mask")]
    [SerializeField] private Animator maskAnim;

    private PlayerInput playerInput;

    private string continueAction;
    private float actualProgress;

    private void OnEnable()
    {
        playerInput = GameObject.FindGameObjectWithTag("INPUT").GetComponent<PlayerInput>();
        ControlSaver.StaticEvent += ChangeText; 
    }

    private void OnDisable(){ ControlSaver.StaticEvent -= ChangeText;}

    public void Activate()
    {
        loadingScreen.SetActive(true);
        playerInput.SwitchCurrentActionMap("LoadingScreen");
    }

    private void Update()
    {
        float xScale = Mathf.Lerp(progressBar.transform.localScale.x, actualProgress, progressBarSpeed * Time.deltaTime);
        progressBar.transform.localScale = new Vector3(xScale, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        if (playerInput.actions.FindAction("Stop").IsPressed())
        {
            maskAnim.SetBool("Stop", true);
        }
    }

    public bool UpdateProgess(float progress)
    {
        actualProgress = Mathf.Clamp01(progress / 0.9f);
        percentage.text = (int)(Mathf.Clamp01(progress / 0.9f) * 100) + " %";

        bool result = false;

        if (progress >= 0.9f)
        {
            percentage.text = "Press " + continueAction + " to continue";
            loadingTextAnim.enabled = true;

            if (playerInput.actions.FindAction("Continue").IsPressed())
                result = true;
        }

        return result;
    }

    public void ChangeText()
    {
        continueAction = playerInput.actions.FindActionMap("LoadingScreen").FindAction("Continue").bindings[ControlSaver.controlSchemeIndex].path.Split("/")[1];
    }

}