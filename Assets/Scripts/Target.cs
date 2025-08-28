// Assets/Scripts/Target.cs
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("HP（需要几发子弹）")]
    public int hitPoints = 3;  // 默认 3 发

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance?.OnTargetDestroyed();
        }
    }
}
