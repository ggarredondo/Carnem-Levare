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
    public Animator animator;
    public Target target;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HITBOX")
            Debug.Log(other.gameObject);
    }
}
