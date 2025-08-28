using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;     // 2 秒自毁
    public LayerMask destroyOnLayers; // 命中这些层就销毁（Ground 等）

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // 朝着“自身右侧方向”发射（由 FirePoint 旋转确定）
        rb.velocity = transform.right * speed;
        Invoke(nameof(SelfDestruct), lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 命中 Target：让它受伤/销毁
        var tgt = other.GetComponent<Target>();
        if (tgt != null)
        {
            tgt.TakeDamage(1);
            SelfDestruct();
            return;
        }

        // 命中地面等层 → 直接销毁子弹
        if (((1 << other.gameObject.layer) & destroyOnLayers) != 0)
        {
            SelfDestruct();
        }
    }

    void SelfDestruct()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
