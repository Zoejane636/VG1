// Assets/Scripts/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI goalText;       // Canvas/GoalText
    public GameObject winPanel;            // Canvas/WinPanel
    public TextMeshProUGUI winLabel;       // Canvas/WinLabel
    public Button restartButton;           // Canvas/RestartButton（在 Canvas 根）

    private int totalTargets;
    private int destroyed;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // 基础状态
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 统计关卡目标数
        totalTargets = FindObjectsOfType<Target>(true).Length;
        destroyed = 0;
        UpdateUI();

        // UI 初始隐藏
        if (winPanel)  winPanel.SetActive(false);
        if (winLabel)  winLabel.gameObject.SetActive(false);

        // —— 自动查找并绑定 Restart 按钮（你就不用手动绑了）——
        if (restartButton == null)
        {
            var go = GameObject.Find("RestartButton");
            if (go) restartButton = go.GetComponent<Button>();
        }
        if (restartButton)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartLevel);
        }
    }

    // 被 Target 调用：击碎一个目标
    public void OnTargetDestroyed()
    {
        destroyed++;
        UpdateUI();
        if (destroyed >= totalTargets)
            Win();
    }

    private void UpdateUI()
    {
        if (goalText) goalText.text = $"Targets: {destroyed}/{totalTargets}";
    }

    private void Win()
    {
        if (winPanel)  winPanel.SetActive(true);   // 显示遮罩
        if (winLabel)
        {
            winLabel.text = "YOU WIN!";
            winLabel.gameObject.SetActive(true);
        }
        // 暂停游戏，但 UI 可以点
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartLevel()
    {
        // 解除暂停并重载场景
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
