using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private SpriteRenderer mySR;
    private bool isTransforming = false;

    [SerializeField] private GunData gunData;

    [SerializeField] private float targetScale = 2f; // Kích thước tối đa
    [SerializeField] private float scaleDuration = 0.5f; // Thời gian phóng to
    [SerializeField] private float moveDuration = 1f;    // Thời gian di chuyển tới Human
    [SerializeField] private float rotateSpeed = 360f;   // Độ xoay mỗi giây



    private void Start()
    {
        mySR = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTransforming && collision.GetComponent<Bullet>() != null)
        {
            StartCoroutine(TransformAndMoveToHuman());
        }

        if(collision.GetComponent<Human>() != null)
        {
            Human human = collision.GetComponent<Human>();
            human.ChangeGun(gunData);
            Destroy(gameObject);
        }
    }

    private IEnumerator TransformAndMoveToHuman()
    {
        isTransforming = true;

        // Phóng to bằng Lerp
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one * targetScale;
        float elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / scaleDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;

        // Vừa xoay vừa bay tới Human bằng Lerp
        Vector3 startPos = transform.position;
        Vector3 endPos = Human.instance.transform.position;
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
}
