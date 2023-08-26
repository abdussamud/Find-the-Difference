using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI gui;

    public GameObject gameplayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private Slider healthSlider;
    public CompletionDot[] completionDot;
    private GameController Gc => GameController.gc;
    private GameManager Gm => GameManager.Instance;
    private const float DURATION = 0.4f;


    private void Awake()
    {
        gui = this;
    }

    private IEnumerator SetHealthBar(float health, float duration = 0.4f)
    {
        float startBarFillAmount = healthSlider.value;
        float endBarFillAmount = health / Gc.maxHealth;

        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1)
        {
            healthSlider.value = Mathf.Lerp(startBarFillAmount, endBarFillAmount, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        healthSlider.value = endBarFillAmount;
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

    public void GoNextLevel()
    {
        Gm.level++;
        Gm.nextScene = "Gameplay";
        Gm.loadingDuration = 0.5f;
        SceneManager.LoadScene("Loading");
    }

    public void GameWon()
    {
        Invoke(nameof(ActivateGameWon), 1.2f);
    }

    public void GoHome()
    {
        Gm.nextScene = "Main Menu";
        Gm.loadingDuration = 1.5f;
        SceneManager.LoadScene("Loading");
    }

    private void ActivateGameWon()
    {
        gameWonPanel.SetActive(true);
    }
}
