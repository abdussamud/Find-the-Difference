using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance { get; private set; }

    public GameObject gameplayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private Image healthBar;
    public CompletionDot[] completionDot;
    private const float DURATION = 0.4f;


    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator SetHealthBar(float health, float duration = 0.4f)
    {
        float startBarFillAmount = healthBar.fillAmount;
        float endBarFillAmount = health / GameController.Instance.maxHealth;

        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1)
        {
            healthBar.fillAmount = Mathf.Lerp(startBarFillAmount, endBarFillAmount, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        healthBar.fillAmount = endBarFillAmount;
        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void SetHealthBar(float health)
    {
        _ = StartCoroutine(SetHealthBar(health, DURATION));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GameWon()
    {
        Invoke(nameof(ActivateGameWon), 1.2f);
    }

    private void ActivateGameWon()
    {
        gameWonPanel.SetActive(true);
    }
}
