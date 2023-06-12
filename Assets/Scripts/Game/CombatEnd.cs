using UnityEngine;
using System.Threading.Tasks;

public class CombatEnd : MonoBehaviour
{
    private Player player;
    private Enemy enemy;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    private void Start()
    {
        enemy.StateMachine.KOState.OnEnter += Victory;
        player.StateMachine.KOState.OnEnter += Defeat;
    }

    private void OnDestroy()
    {
        enemy.StateMachine.KOState.OnEnter -= Victory;
        player.StateMachine.KOState.OnEnter -= Defeat;
    }

    private async void Victory()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(3));

        TransitionPlayer.extraTime = 1;
        TransitionPlayer.text.text = "YOU LIVE";

        AudioController.Instance.uiSfxSounds.Play("PlayGame");
        GameManager.SceneController.NextScene();
    }

    private async void Defeat()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(3));

        TransitionPlayer.extraTime = 2;
        TransitionPlayer.text.text = "YOU DIED";

        AudioController.Instance.uiSfxSounds.Play("BackMenu");
        GameManager.SceneController.PreviousScene();
    }
}