using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Guns/New Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;
    public float cooldown;
    public int bulletsPerShot;
    public int damage;
    public Sprite gunSprite;

    [Header("Fire Point Offset")]
    public Vector2 firePointOffset; // ← vị trí đầu nòng so với tâm sprite
}
