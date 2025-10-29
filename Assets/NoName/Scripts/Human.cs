using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public static Human instance;

    public List<GameObject> magazine = new List<GameObject>();
    [SerializeField] private GunData currentGun;
    [SerializeField] private SpriteRenderer gunSprite;
    [SerializeField] private float attackCheckDistance ;

    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Transform shootEffectTransform;

    [Header("Stats")]
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private Slider healthBarSlide;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] Transform firePoint;
    private float timer;

    public static Action updateHealthBar;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;


        updateHealthBar += UpdateHealthBar;

    }

    private void OnDestroy()
    {
        updateHealthBar -= UpdateHealthBar;

    }
    private void Start()
    {
        healthBarSlide = GetComponentInChildren<Slider>();

        timer = cooldown;
        currentHealth = maxHealth;

        healthBarSlide.value = currentHealth;

        int selectedGunIndex = PlayerPrefs.GetInt("SelectedGunIndex", 0);
        GunData[] allGuns = GameManager.instance.allGuns; // hoặc truyền từ ShopManager

        if (allGuns != null && selectedGunIndex < allGuns.Length)
        {
            currentGun = allGuns[selectedGunIndex];
            firePoint.localPosition = allGuns[selectedGunIndex].firePointOffset;
            shootEffectTransform.localPosition = allGuns[selectedGunIndex].firePointOffset;

            gunSprite.sprite = currentGun.gunSprite;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (EnemyDetected())
        {
            float baseCooldown = currentGun != null ? currentGun.cooldown : cooldown;

            // Nếu có tiếp tế, giảm cooldown theo cấp số mũ (giảm dần nhẹ hơn)
            float currentCooldown = magazine.Count > 0
                ? baseCooldown / Mathf.Pow(1.02f, magazine.Count)
                : baseCooldown;


            if (timer <= 0)
            {
                Shoot();
                timer = currentCooldown;
            }
        }
    }

    public void ChangeGun(GunData _gunData)
    {
        currentGun = _gunData;
        gunSprite.sprite = _gunData.gunSprite;
        firePoint.localPosition = _gunData.firePointOffset;
        shootEffectTransform.localPosition = _gunData.firePointOffset;
    }

    private bool EnemyDetected() => Physics2D.Raycast(transform.position, Vector2.right, attackCheckDistance, whatIsEnemy);

    private void Shoot()
    {
        var main = shootEffect.main;
        float duration = main.duration;
        float baseCooldown = currentGun != null ? currentGun.cooldown : cooldown;

        // Nếu tốc độ bắn cao hơn tốc độ "tắt" particle → chỉ Play, không Stop
        if (baseCooldown < duration * 0.8f)
        {
            // Bắn nhanh → giữ hiệu ứng sáng liên tục
            if (!shootEffect.isPlaying)
                shootEffect.Play();
        }
        else
        {
            // Bắn bình thường → nháy từng phát
            shootEffect.Stop();
            shootEffect.Play();
        }

        // Phần còn lại giữ nguyên
        int bulletsToShoot = currentGun != null ? currentGun.bulletsPerShot : 1;
        int bulletDamage = currentGun != null ? currentGun.damage : damage;

        for (int i = 0; i < bulletsToShoot; i++)
        {
            GameObject bullet = bulletPool.GetObject();
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<HumanBullet>().Damage(bulletDamage);
        }

        if (magazine.Count > 0)
            magazine.RemoveAt(0);
    }


    public void UpdateHealthBar()
    {
        healthBarSlide.value = currentHealth/maxHealth;
    }

    public void AddBulletToMagazine(GameObject bullet)
    {
        magazine.Add(bullet);
    }


    public ObjectPool GetObjectPool()
    {
        return bulletPool;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        updateHealthBar?.Invoke();

        if (currentHealth <= 0)
        {
            GameManager.instance.SetGameState(GameManager.GameState.Gameover);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * attackCheckDistance);
    }
}
