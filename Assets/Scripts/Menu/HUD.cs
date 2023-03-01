using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Axis { x = 0, y = 1, z = 2 }

public class HUD : MonoBehaviour
{
    private Character player, enemy;
    [SerializeField] private Image playerStaminaBar, enemyStaminaBar;
    [SerializeField] private TMP_Text playerNumber, enemyNumber;
    [Tooltip("Speed at which stamina bars update")] [SerializeField] private float updateSpeed = 5f;
    private float currentP, currentE;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
        currentP = player.MaxStamina;
        currentE = enemy.MaxStamina;
    }

    /// <summary>
    /// Update image scale in one axis corresponding to a percentage given by
    /// a current and max value.
    /// </summary>
    private void UpdateBar(float value, float maxValue, Image bar, Axis axis, TMP_Text text) {
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        newScale[(int) axis] = value / maxValue;
        bar.transform.localScale = newScale;
        text.text = Mathf.Round(value) + "/" + maxValue;
    }

    private void Update()
    {
        currentP = Mathf.Lerp(currentP, player.Stamina, updateSpeed * Time.deltaTime);
        UpdateBar(currentP, player.MaxStamina, playerStaminaBar, Axis.x, playerNumber);

        currentE = Mathf.Lerp(currentE, enemy.Stamina, updateSpeed * Time.deltaTime);
        UpdateBar(currentE, enemy.MaxStamina, enemyStaminaBar, Axis.x, enemyNumber);
    }
}
