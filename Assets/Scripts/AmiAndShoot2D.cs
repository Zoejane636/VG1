using UnityEngine;

public class AimAndShoot2D : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;          // 枪口
    public GameObject bulletPrefab;      // 子弹预制

    [Header("Fire")]
    public float fireRate = 6f;          // 次/秒（按住左键连射）
    private float fireCooldown = 0f;

    void Update()
    {
        // 1) 旋转枪朝向鼠标
        Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0f;
        Vector2 dir = (m - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 2) 开火（左键）
        fireCooldown -= Time.deltaTime;
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !firePoint) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
