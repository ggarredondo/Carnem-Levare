using UnityEngine;
using System.Threading.Tasks;

public class GameCycle : MonoBehaviour, IObjectInitialize
{
    private Player player;
    private Enemy enemy;

    [Header("Requirements")]
    [SerializeField] private EnemyLoader enemyLoader;
    [SerializeField] private RewardGenerator rewardGenerator;

    [Header("Parameters")]
    [SerializeField] private float waitAfterDeath;
    [SerializeField] private float waitAfterReward;

    public void Initialize(ref GameObject player, ref GameObject enemy)
    {
        enemyLoader.Initialize();
        enemy = enemyLoader.LoadEnemy();

        this.player = player.GetComponent<Player>();
        this.enemy = enemy.GetComponent<Enemy>();

        this.enemy.StateMachine.KOState.OnEnter += Victory;
        this.player.StateMachine.KOState.OnEnter += Defeat;
    }

    private void OnDestroy()
    {
        enemy.StateMachine.KOState.OnEnter -= Victory;
        player.StateMachine.KOState.OnEnter -= Defeat;
    }

    private async void Victory()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(waitAfterDeath));
        GameManager.InputUtilities.EnablePlayerInput(false);

        DataSaver.Game.enemyPrefabNames.RemoveAt(enemyLoader.GetActualEnemyId());

        if (DataSaver.Game.enemyPrefabNames.Count != 0)
        {
            enemy.EnemyDrops.ForEach(m =>
            {
                DataSaver.Game.moves.Add(m);
                DataSaver.Game.newMoves.Add(true);
            });

            await rewardGenerator.GenerateMove(enemy.EnemyDrops);
            await Task.Delay(System.TimeSpan.FromSeconds(waitAfterReward));

            TransitionPlayer.extraTime = 1;
            TransitionPlayer.text.text = "YOU LIVE";

            GameManager.AudioController.Play("PlayGame");
            GameManager.Scene.NextScene();
        }
        else
        {
            TransitionPlayer.extraTime = 1;
            TransitionPlayer.text.text = "YOU'RE THE CARNEM LEVARE";

            GameManager.Save.SetDefault();
            GameManager.AudioController.Play("PlayGame");
            GameManager.Scene.PreviousScene();
        }
    }

    private async void Defeat()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(3));

        TransitionPlayer.extraTime = 2;
        TransitionPlayer.text.text = "YOU DIED";

        GameManager.AudioController.Play("BackMenu");
        GameManager.Scene.PreviousScene();
    }
}
