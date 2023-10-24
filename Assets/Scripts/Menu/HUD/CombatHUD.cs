using UnityEngine;

public class CombatHUD : MonoBehaviour
{
    private CharacterStats player, enemy;
    [SerializeField] private SelfUpdatingBar playerHealthBar, playerStaminaBar, enemyHealthBar, enemyStaminaBar;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().CharacterStats;
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>().CharacterStats;
    }

    private void Update()
    {
        playerHealthBar.UpdateBar(player.Health, player.MaxHealth);
        playerStaminaBar.UpdateBar(player.Stamina, player.MaxStamina);
        enemyHealthBar.UpdateBar(enemy.Health, enemy.MaxHealth);
        enemyStaminaBar.UpdateBar(enemy.Stamina, enemy.MaxStamina);
    }
}
