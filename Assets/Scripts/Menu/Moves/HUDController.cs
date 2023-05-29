using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private List<GameObject> HUDMenus;

    [Header("Parameters")]
    [Range(0,1)] [SerializeField] private float lerpDuration;


    private Player player;
    private GameObject enemy;
    private CameraController cameraController;
    private bool cameraChanged;

    private int actualHUDMenu = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        cameraController = GameObject.FindGameObjectWithTag("CAMERA").GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        inputReader.SelectMenuEvent += ChangeHUDMenu;
        inputReader.ChangeCamera += ChangeCamera;
    }

    private void OnDisable()
    {
        inputReader.SelectMenuEvent -= ChangeHUDMenu;
        inputReader.ChangeCamera -= ChangeCamera;
    }

    private void ChangeCamera()
    {
        if (!cameraChanged)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            cameraController.changeVirtualCamera = CameraType.GOPRO;
            enemy.SetActive(false);
        }
        else
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            cameraController.changeVirtualCamera = CameraType.DEFAULT;
            enemy.SetActive(true);
            AudioController.Instance.gameSfxSounds.StopAllSounds();
        }

        cameraChanged = !cameraChanged;
    }

    public async void ChangeHUDMenu()
    {
        CanvasGroup actualCanvas = HUDMenus[actualHUDMenu].GetComponent<CanvasGroup>();

        if (actualCanvas != null)
            await LerpCanvasAlpha(actualCanvas, 0, 0.1f);

        HUDMenus.ForEach(m => m.SetActive(false));
        actualHUDMenu = (actualHUDMenu + 1) % HUDMenus.Count;
        HUDMenus[actualHUDMenu].SetActive(true);

        actualCanvas = HUDMenus[actualHUDMenu].GetComponent<CanvasGroup>();

        if (actualCanvas != null)
            await LerpCanvasAlpha(actualCanvas, 1, lerpDuration);
    }

    private async Task LerpCanvasAlpha(CanvasGroup canvasGroup,float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            await Task.Yield();
        }

        canvasGroup.alpha = targetAlpha;
    }
}
