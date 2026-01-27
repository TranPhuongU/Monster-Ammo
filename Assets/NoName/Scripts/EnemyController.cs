using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float attackCheck;
    public GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Stats")]
    [SerializeField] private Vector2 minMaxSpeed;
    [SerializeField] private float maxHealth;
    public int damage;
    private float currentHealth;
    private float moveSpeed;

    private Slider slider;
    private Rigidbody2D rb;
    private Animator anim;



    public static Action updateHealthBar;

    private void Awake()
    {
        updateHealthBar += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        updateHealthBar -= UpdateHealthBar;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slider = GetComponentInChildren<Slider>();
        anim = GetComponentInChildren<Animator>();
        moveSpeed = UnityEngine.Random.Range(minMaxSpeed.x, minMaxSpeed.y);
        currentHealth = maxHealth;
        slider.value = currentHealth;
    }
    
    private void Update()
    {
        rb.velocity = Vector3.left * moveSpeed;

        if (PlayerDetected())
        {
            anim.SetTrigger("Attack");
            rb.velocity = Vector2.zero;
        }

        if(Human.instance.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        slider.value = currentHealth / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<HumanBullet>() != null)
        {
            int damageAmount = collision.GetComponent<HumanBullet>().GetDamage();
            
            currentHealth -= damageAmount;

            updateHealthBar?.Invoke();
            StartCoroutine(HurtFX());

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                DataManager.instance.AddCoins(10);
            }
        }
    }

    private bool PlayerDetected() => Physics2D.Raycast(transform.position, Vector2.left, attackCheck, whatIsPlayer);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawRay(transform.position, Vector2.left * attackCheck);
    }

    private IEnumerator HurtFX()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;


    }

    public Transform FirePoint()
    {
        return firePoint;
    }
}
