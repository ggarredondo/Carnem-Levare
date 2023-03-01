using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Axis { x = 0, y = 1, z = 2 }

public class HUD : MonoBehaviour
{
    private Character player, enemy;
    [SerializeField] private Image playerStaminaBar, enemyStaminaBar;
    [SerializeField] private TMP_Text playerNumber, enemyNumber;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    /// <summary>
    /// Update image scale in one axis corresponding to a percentage given by
    /// a current and max value.
    /// </summary>
    private void UpdateBar(float value, float maxValue, Image bar, Axis axis) {
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        newScale[(int) axis] = value / maxValue;
        bar.transform.localScale = newScale;
    }

    private void Update()
    {
        UpdateBar(player.Stamina, player.MaxStamina, playerStaminaBar, Axis.x);
        playerNumber.text = (int) player.Stamina + "/" + player.MaxStamina;
        UpdateBar(enemy.Stamina, enemy.MaxStamina, enemyStaminaBar, Axis.x);
        enemyNumber.text = (int) enemy.Stamina + "/" + enemy.MaxStamina;
    }
}
