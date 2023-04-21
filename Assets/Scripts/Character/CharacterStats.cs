using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float stamina, maxStamina;
    [SerializeField] private float characterDamage;

    [Tooltip("Percentage of stamina damage taken when blocking")]
    [SerializeField] [Range(0f, 1f)] private float blockingMultiplier;

    [Tooltip("How quickly time disadvantage decreases through consecutive hits (combo decay in ms x number of hits)")]
    [SerializeField] private float comboDecay = 200f;

    [SerializeField] [InitializationField] private float height = 1f, mass = 1f, drag;

    private void Awake()
    {
        stamina = maxStamina;
        transform.localScale *= height;
        GetComponent<Rigidbody>().mass = mass;
        GetComponent<Rigidbody>().drag = drag;
    }

    public float CalculateAttackDamage(float baseDamage) 
    {
        return baseDamage + characterDamage;
    }
    public void AddToStamina(float addend) 
    {
        stamina = Mathf.Clamp(stamina + addend, 0f, maxStamina);
    }

    public float Stamina { get => stamina; }
    public float MaxStamina { get => maxStamina; }
    public float ComboDecay { get => comboDecay; }
}
