using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI gui;

    public int hintCount;
    public Image healthImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hintCountText;
    public GameObject hintAdsGO;
    public GameObject gameWonPanel;
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;
    public CompletionDot[] completionDot;
    private const float DURATION = 0.4f;
    private GameController Gc => GameController.gc;
    private GameManager Gm => GameManager.Instance;

    private void Awake()
    {
        gui = this;
    }

    private void Start()
    {
        levelText.text = "Level " + (1 + Gm.level).ToString();
        hintCountText.text = hintCount.ToString();
        hintAdsGO.SetActive(hintCount <= 0);
        AdjustOrthographicSize();
    }

    private void AdjustOrthographicSize()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            const float widthConst = 2.75f;
            mainCamera.orthographicSize = widthConst / mainCamera.aspect;
        }
    }

    private IEnumerator SetHealthBar(float health, float duration = 0.4f)
    {
        float startBarFillAmount = healthImage.fillAmount;
        float endBarFillAmount = health / Gc.maxHealth;

        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1)
        {
            healthImage.fillAmount = Mathf.Lerp(startBarFillAmount, endBarFillAmount, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        healthImage.fillAmount = endBarFillAmount;
        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
            AdsManager.adM.LevelCompleted();
        }
    }

    public void SetHealthBar(float health)
    {
        _ = StartCoroutine(SetHealthBar(health, DURATION));
    }

    public void PauseGame()
    {

    }

    public void ShowHint()
    {
        if (Gc.gameOver) { return; }
        Gc.ShowHint();
        hintCount -= hintCount > 0 ? 1 : 0;
        hintCountText.text = hintCount.ToString();
        hintAdsGO.SetActive(hintCount <= 0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ReplayGame()
    {
        Gm.level -= Gm.level < 14 ? 1 : 0;
        Gm.nextScene = "Gameplay";
        Gm.loadingDuration = 0.5f;
        SceneManager.LoadScene("Loading");
    }

    public void GoNextLevel()
    {
        Gm.nextScene = "Gameplay";
        Gm.loadingDuration = 0.5f;
        SceneManager.LoadScene("Loading");
    }

    public void SkipLevel()
    {
        Gm.level += Gm.level < 14 ? 1 : 0;
        PlayerPrefs.SetInt("level", Gm.level);
        PlayerPrefs.Save();
        Gm.nextScene = "Gameplay";
        Gm.loadingDuration = 0.5f;
        SceneManager.LoadScene("Loading");
    }

    public void GameWon()
    {
        Gm.level += Gm.level < 14 ? 1 : 0;
        PlayerPrefs.SetInt("level", Gm.level);
        PlayerPrefs.Save();
        Invoke(nameof(ActivateGameWon), 1.2f);
    }

    public void GoHome()
    {
        Gm.nextScene = "Main Menu";
        Gm.loadingDuration = 1.5f;
        SceneManager.LoadScene("Loading");
    }

    public void ActivateDot(int index)
    {
        for (int i = 0; i < index; i++)
        {
            completionDot[i].innerGO.SetActive(true);
        }
    }

    private void ActivateGameWon()
    {
        AdsManager.adM.LevelCompleted();
        gameWonPanel.SetActive(true);
    }
}
