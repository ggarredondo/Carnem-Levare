using UnityEngine;
using System.Threading.Tasks;
using LerpUtilities;
using System.Threading;

public class GameCycle : MonoBehaviour, IObjectInitialize
{
    private Player player;
    private Enemy enemy;

    [Header("Requirements")]
    [SerializeField] private EnemyLoader enemyLoader;
    [SerializeField] private RewardGenerator rewardGenerator;
    [SerializeField] private Light ambientLight;
    [SerializeField] private Light pointLight;

    [Header("Parameters")]
    [SerializeField] private float waitAfterDeath;
    [SerializeField] private float waitAfterReward;

    private float localHealth;
    private CancellationTokenSource playerCancellationTokenSource;
    private CancellationTokenSource enemyCancellationTokenSource;

    public void Initialize(ref GameObject player, ref GameObject enemy)
    {
        enemyLoader.Initialize();
        enemy = enemyLoader.LoadEnemy();

        this.player = player.GetComponent<Player>();
        this.enemy = enemy.GetComponent<Enemy>();

        this.enemy.StateMachine.KOState.OnEnter += Victory;
        this.player.StateMachine.KOState.OnEnter += Defeat;
        this.player.StateMachine.HurtState.OnEnter += HurtPlayer;
        this.enemy.StateMachine.HurtState.OnEnter += HurtEnemy;

        playerCancellationTokenSource = new();
        enemyCancellationTokenSource = new();
    }

    private async void HurtEnemy()
    {
        enemyCancellationTokenSource.Dispose();
        enemyCancellationTokenSource = new();
        CancellationToken cancellationToken = enemyCancellationTokenSource.Token;

        localHealth = enemy.CharacterStats.Health / (float)enemy.CharacterStats.MaxHealth;
        try 
        { 
        await Lerp.Value_Cancel(pointLight.intensity,
                                20 + 20 * (1 - localHealth),
                                (i) => pointLight.intensity = i, 2f, cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    private async void HurtPlayer()
    {
        playerCancellationTokenSource.Dispose();
        playerCancellationTokenSource = new();
        CancellationToken cancellationToken = playerCancellationTokenSource.Token;

        localHealth = player.CharacterStats.Health / (float)player.CharacterStats.MaxHealth;

        try
        { 
        await Lerp.Value_Cancel(ambientLight.color,
                                new Color(localHealth, ambientLight.color.g, ambientLight.color.b, ambientLight.color.a),
                                (c) => ambientLight.color = c, 2f, cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    private void OnDisable()
    {
        playerCancellationTokenSource?.Cancel();
        enemyCancellationTokenSource?.Cancel();
    }

    private void OnDestroy()
    {
        enemy.StateMachine.KOState.OnEnter -= Victory;
        player.StateMachine.KOState.OnEnter -= Defeat;
        player.StateMachine.HurtState.OnEnter -= HurtPlayer;
        enemy.StateMachine.HurtState.OnEnter -= HurtEnemy;
    }

    private async void Victory()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(waitAfterDeath));
        GameManager.Input.EnablePlayerInput(false);

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

            GameManager.Audio.Play("PlayGame");
            GameManager.Scene.NextScene();
        }
        else
        {
            TransitionPlayer.extraTime = 1;
            TransitionPlayer.text.text = "YOU'RE THE CARNEM LEVARE";

            GameManager.Save.SetDefault();
            GameManager.Audio.Play("PlayGame");
            GameManager.Scene.PreviousScene();
        }
    }

    private async void Defeat()
    {
        await Task.Delay(System.TimeSpan.FromSeconds(3));

        TransitionPlayer.extraTime = 2;
        TransitionPlayer.text.text = "YOU DIED";

        GameManager.Save.SetDefault();
        GameManager.Audio.Play("BackMenu");
        GameManager.Scene.PreviousScene();
    }
}
