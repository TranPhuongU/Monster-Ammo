using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float moveSpeed = 5f;

    private bool isMovingThroughPath = false;
    private bool canAddBullet;
    private List<Doors> hitDoor = new List<Doors>();


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector2.up * bulletSpeed;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMovingThroughPath && collision.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            var pathProvider = collision.GetComponent<PathProvider>();
            if (pathProvider != null)
            {
                GetComponent<Collider2D>().enabled = false;
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                isMovingThroughPath = true;

                StartCoroutine(MoveThroughPath(pathProvider.GetPathPoints()));
            }
        }

        if (collision.GetComponent<Doors>() != null)
        {
            Doors doorTarget = collision.GetComponent<Doors>();
            int bonusAmount = doorTarget.GetBonusAmount();
            BonusType bonusType = doorTarget.GetBonusType();

            if (hitDoor.Contains(doorTarget))
                return;

            ApplyBonus(bonusType, bonusAmount, doorTarget);

            hitDoor.Add(doorTarget);
        }
    }

    private IEnumerator MoveThroughPath(List<Transform> pathPoints)
    {
        foreach (Transform point in pathPoints)
        {
            yield return MoveTo(point.position);
        }

        CanonController.instance.BulletPool().ReturnObject(gameObject);
        Human.instance.AddBulletToMagazine(gameObject);
    }


    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
    }

    public void ApplyBonus(BonusType bonusType, int bonusAmount, Doors doors)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                if(canAddBullet)
                {
                    AddBullets(bonusAmount);
                }
                break;
            case BonusType.Difference:
                RemoveBullet(bonusAmount, doors);
                break;
        }
    }

    private void AddBullets(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CanonController.instance.AddBullet(transform, i, amount); // truyền thêm index và total
        }
    }

    private void RemoveBullet(int bonusAmount, Doors doors)
    {
        if(doors.doorBonusAmount <= 0)
            return;

        CanonController.instance.BulletPool().ReturnObject(gameObject);
        doors.doorBonusAmount--;
        Doors.updateDoorText?.Invoke();
    }

    public void ResetBullet()
    {
        // Reset collider, rotation, velocity, isKinematic, flags
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        transform.rotation = Quaternion.identity;
        rb.isKinematic = false;
        isMovingThroughPath = false;
        hitDoor.Clear();
    }


    public void CanAddBullet(bool value)
    {
        canAddBullet = value;
    }
}
