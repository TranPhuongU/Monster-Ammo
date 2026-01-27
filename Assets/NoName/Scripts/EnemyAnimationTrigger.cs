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

    public void DestroyMe()
    {
        Destroy(enemy.gameObject);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(enemy.bulletPrefab, enemy.FirePoint().position, Quaternion.identity);

        bullet.GetComponent<EnemyBullet>().Setup(enemy.damage);
    }
}
