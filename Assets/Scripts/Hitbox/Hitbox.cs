using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [System.NonSerialized] public Power power = Power.Light;
    [System.NonSerialized] public float damage = 0f;
    [System.NonSerialized] public bool hit = false;
    [System.NonSerialized] public bool unblockable = false;

    public void Activate(bool activated) {
        transform.gameObject.SetActive(activated && !hit);
    }
}
