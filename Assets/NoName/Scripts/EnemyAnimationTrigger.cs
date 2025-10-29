using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    public void AttackTrigger()
    {
        Human.instance.TakeDamage(enemy.damage);
    }
}
