using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float timeDestroyBullet = 3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        float angleOffset = Random.Range(-5f, 7f); // lệch từ -10 đến +10 độ
        Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * Vector2.right;

        rb.velocity = direction.normalized * moveSpeed;
    }


    private void Update()
    {
        timeDestroyBullet -= Time.deltaTime;

        if(timeDestroyBullet <= 0)
        {
            Human.instance.GetObjectPool().ReturnObject(gameObject);
            timeDestroyBullet = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyController>() != null)
        {
            Human.instance.GetObjectPool().ReturnObject(gameObject);
        }
    }

    public int Damage(int dmg)
    {
        return damage = dmg;
    }
    public int GetDamage()
    {
        return damage;
    }
}
