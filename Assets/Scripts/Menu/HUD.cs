using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Axis { x = 0, y = 1, z = 2 }

public class HUD : MonoBehaviour
{
    private CharacterStats player, enemy;
    [SerializeField] private Image playerInstant, playerAnimated, enemyInstant, enemyAnimated;
    [SerializeField] private TMP_Text playerNumber, enemyNumber;
    [Tooltip("Speed at which stamina bars update")] [SerializeField] private float updateSpeed = 5f;
    private float currentP, currentE;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().Stats;
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>().Stats;
        currentP = player.MaxStamina;
        currentE = enemy.MaxStamina;
    }

    /// <summary>
    /// Update image scale in one axis corresponding to a percentage given by
    /// a current and max value.
    /// </summary>
    private void UpdateBar(float value, float maxValue, Image bar, Axis axis) {
        Vector3 newScale = new Vector3(1f, 1f, 1f);
        newScale[(int) axis] = value / (maxValue == 0f ? 1f : maxValue);
        bar.transform.localScale = newScale;
    }

    private void UpdateCharacterBar(CharacterStats charac, ref float current, Image instant, Image animated, TMP_Text number) {
        current = Mathf.Lerp(current, charac.Stamina, updateSpeed * Time.deltaTime);
        UpdateBar(current, charac.MaxStamina, animated, Axis.x);
        UpdateBar(charac.Stamina, charac.MaxStamina, instant, Axis.x);
        number.text = charac.Stamina + "/" + charac.MaxStamina;
    }

    private void Update()
    {
        UpdateCharacterBar(player, ref currentP, playerInstant, playerAnimated, playerNumber);
        UpdateCharacterBar(enemy, ref currentE, enemyInstant, enemyAnimated, enemyNumber);
    }
}
