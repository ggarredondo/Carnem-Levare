using UnityEngine;

public enum Target : uint
{
    LeftHead = 0,
    MidHead = 1,
    RightHead = 2,
    LeftBody = 3,
    MidBody = 4,
    RightBody = 5
}

public class Hurtbox : MonoBehaviour
{
    public Character character;
    public Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        character.getAnimator.SetTrigger("hurt");
        character.getAnimator.SetFloat("hurt_target", (float) target);
        character.getAnimator.SetFloat("hurt_power", 0f);
    }
}
