using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public int maxJumps = 2;              // 允许的最大跳跃次数（两跳）

    [Header("Ground Check")]
    public Transform groundCheck;         // 放在脚底
    public float groundCheckDist = 0.06f; // 射线长度，越小越严
    public LayerMask groundLayer;         // 只勾 Ground 层

    private Rigidbody2D rb;
    private int jumpsUsed = 0;            // 已用跳数（关键！）
    private bool grounded = false;
    private bool groundedLast = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 水平移动
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // 落地检测：用向下 Raycast，避免 Overlap 抖动重置
        grounded = IsGrounded();

        // 只有“未接地 → 接地”的瞬间才重置跳数
        if (grounded && !groundedLast)
            jumpsUsed = 0;

        groundedLast = grounded;

        // 跳跃：严格限制两跳（或 maxJumps）
        if (Input.GetKeyDown(KeyCode.Space) && jumpsUsed < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);               // 清纵速，手感更干脆
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsUsed++;                                                // 计数 +1
        }

        // 掉坑重载（按需调整阈值）
        if (transform.position.y < -20f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 用 Raycast 向下打 Ground 层，返回是否接地
    private bool IsGrounded()
    {
        if (!groundCheck) return false;
        // 只在下落/静止时判定接地，更稳
        if (rb.velocity.y > 0.01f) return false;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
        return hit.collider != null;
    }

    // 在 Scene 里可视化射线
    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDist);
    }
}
