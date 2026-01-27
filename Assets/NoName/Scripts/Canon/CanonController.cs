using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanonController : MonoBehaviour
{
    public static CanonController instance;

    [SerializeField] private Transform wheelRight;
    [SerializeField] private Transform wheelLeft;

    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletAmount;

    [SerializeField] private ParticleSystem fireEffect;

    [Header("Fire info")]
    [SerializeField] private float fireCooldown;
    private float fireTimer;

    [Header("Settings")]
    [SerializeField] private float roadWitdth;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wheelRotationSpeed = 800f; // tốc độ xoay bánh

    private Vector3 clickedScreenPosition;
    private Vector3 clickedPlayerPosition;
    private Vector3 lastPosition; // để tính quãng đường di chuyển mỗi frame

    private Animator anim;
    private bool canMove;

    [SerializeField] private TextMeshPro bulletAmountText;

    public static Action onShoot;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        canMove = true;
        fireTimer = fireCooldown;
        lastPosition = transform.position;

        UpdateBulletAmountUI();

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (canMove)
        {
            ManageControl();
            RotateWheels();
        }
    }

    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            StartMoving();
        else if (gameState == GameManager.GameState.Gameover || gameState == GameManager.GameState.LevelComplete)
            StopMoving();
    }

    private void ManageControl()
    {
        if (IsPointerOverUI())
            return;


        if (Input.GetMouseButtonDown(0))
        {
            clickedScreenPosition = Input.mousePosition;
            clickedPlayerPosition = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            float xScreenDifference = Input.mousePosition.x - clickedScreenPosition.x;

            xScreenDifference /= Screen.width;
            xScreenDifference *= slideSpeed;

            Vector3 position = transform.position;
            position.x = clickedPlayerPosition.x + xScreenDifference;
            position.x = Mathf.Clamp(position.x, -roadWitdth, roadWitdth);

            transform.position = position;

            if (fireTimer <= 0 & bulletAmount > 0)
            {
                fireTimer = fireCooldown;
                Shoot();
            }
        }
    }

    private void RotateWheels()
    {
        // Tính hướng di chuyển và khoảng cách trong frame này
        float deltaX = transform.position.x - lastPosition.x;
        lastPosition = transform.position;

        // Nếu không di chuyển thì không cần xoay
        if (Mathf.Abs(deltaX) < 0.0001f) return;

        // Tốc độ xoay tỉ lệ với quãng đường di chuyển
        float rotationAmount = deltaX * wheelRotationSpeed;

        // Xoay bánh – trái ngược chiều nhau để nhìn tự nhiên
        wheelLeft.Rotate(Vector3.back, rotationAmount);
        wheelRight.Rotate(Vector3.back, rotationAmount);
    }

    void Shoot()
    {
        anim.SetTrigger("Fire");

        fireEffect.Stop();
        fireEffect.Play();

        onShoot?.Invoke();

        bulletAmount--;

        UpdateBulletAmountUI();

        GameObject bullet = bulletPool.GetObject();
        bullet.transform.position = firePoint.position;
        bullet.GetComponent<Bullet>().ResetBullet();
        bullet.GetComponent<Bullet>().CanAddBullet(true);
    }

    private void UpdateBulletAmountUI()
    {
        bulletAmountText.text = bulletAmount.ToString();
    }

    public void AddBullet(Transform bulletPos, int index, int total)
    {
        GameObject bullet = bulletPool.GetObject();

        float spacing = 0.1f;
        float startX = bulletPos.position.x - ((total - 1) * spacing / 2);
        float xPos = startX + index * spacing;

        bullet.transform.position = new Vector2(xPos, bulletPos.position.y);
        bullet.GetComponent<Bullet>().ResetBullet();
        bullet.GetComponent<Bullet>().CanAddBullet(false);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * bulletSpeed;
    }

    public ObjectPool BulletPool()
    {
        return bulletPool;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
    if (Input.touchCount > 0)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;
        }
    }
    return false;
#elif UNITY_WEBGL
    // WebGL: Chuột dùng pointerId = -1
    return EventSystem.current.IsPointerOverGameObject(-1);
#else
    return false;
#endif
    }



    private void StartMoving() => canMove = true;
    private void StopMoving() => canMove = false;
}
