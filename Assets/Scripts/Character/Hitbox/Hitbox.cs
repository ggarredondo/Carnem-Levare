using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private AttackMove attackMove;
    private int damageToHealth, damageToStamina;
    private HurtHeight hurtHeight;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(in AttackMove attackMove, int damageToHealth, int damageToStamina)
    {
        this.attackMove = attackMove;
        this.damageToHealth = damageToHealth;
        this.damageToStamina = damageToStamina;
    }

    public void SetActive(bool active) => transform.gameObject.SetActive(active);

    public ref readonly string HurtSound => ref attackMove.HurtSound;
    public ref readonly string BlockedSound => ref attackMove.BlockedSound;
    public ref readonly string StaggerSound => ref attackMove.StaggerSound;
    public ref readonly string KOSound => ref attackMove.KOSound;

    public double HurtStun => attackMove.HurtStun;
    public double BlockedStun => attackMove.BlockedStun;

    public ref readonly CameraEffectsData HurtCameraShake => ref attackMove.HurtCameraShake;
    public ref readonly CameraEffectsData BlockedCameraShake => ref attackMove.BlockedCameraShake;
    public ref readonly CameraEffectsData StaggerCameraShake => ref attackMove.StaggerCameraShake;
    public ref readonly CameraEffectsData KOCameraShake => ref attackMove.KOCameraShake;

    public ref readonly HitStopData HurtHitStop => ref attackMove.HurtHitStop;
    public ref readonly HitStopData BlockedHitStop => ref attackMove.BlockedHitStop;
    public ref readonly HitStopData StaggerHitStop => ref attackMove.StaggerHitStop;
    public ref readonly HitStopData KOHitStop => ref attackMove.KOHitStop;

    public ref readonly Vector3 HurtKnockback => ref attackMove.HurtKnockback;
    public ref readonly Vector3 BlockedKnockback => ref attackMove.BlockedKnockback;

    public float HurtSide => (float) attackMove.HurtSide;
    public float HurtPower => (float) attackMove.HurtPower;
    public float HurtHeight => (float) hurtHeight;
    public void SetHeight(HurtHeight hurtHeight) => this.hurtHeight = hurtHeight;

    public int DamageToHealth => damageToHealth;
    public int DamageToStamina => damageToStamina;
}
