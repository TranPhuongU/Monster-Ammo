using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBullet : MonoBehaviour
{
    private int damage;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Setup(int _damage)
    {
        damage = _damage;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Human.instance.transform.position, 10f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Human>() != null)
        {
            anim.SetTrigger("Explode");
        }
    }

    public void DealDamage()
    {
        Human.instance.TakeDamage(damage);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

}
