using UnityEngine;

public class EnemyAttackManager : AttackManager
{
    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }
}
