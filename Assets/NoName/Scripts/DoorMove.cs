using UnityEngine;

public class DoorMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    private int direction = 1; // 1 = phải, -1 = trái

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Di chuyển theo hướng hiện tại
        rb.velocity = Vector3.right * direction * moveSpeed ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi va chạm với Ground → đổi hướng
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            direction *= -1;
        }
    }
}
