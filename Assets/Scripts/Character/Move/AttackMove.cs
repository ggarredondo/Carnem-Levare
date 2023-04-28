using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double advantageOnBlock;
    [SerializeField] private double advantageOnHit;

    private enum Stagger : int { Light, Medium, Hard }
    [Header("Attack Values")]
    [SerializeField] private string hitboxTag;
    [SerializeField] private float baseDamage;
    [SerializeField] private Stagger baseStagger;
    [SerializeField] private bool unblockable;

    public override void InitMove()
    {
        // hitbox = GameObject.FindGameObjectWithTag(hitboxTag).GetComponent<Hitbox>();
        // hitbox.Set(totalDamage, totalStagger, unblockable, advantages);
    }
    public override void ActivateMove()
    {
        // hitbox.Activate();
    }
    public override void DeactivateMove()
    {
        // hitbox.Deactivate();
    }
    public override void RecoverFromMove()
    {
        // volver a estado walking o blocking
    }
}
